Imports System.IO
Imports GTA

''' <summary>
''' Centralized path configuration for GTA V Enhanced compatibility.
''' Enhanced uses DirectStorage which prevents writing to the game folder,
''' so we use LocalAppData for writable paths on Enhanced.
''' </summary>
Module PathConfig

    Private Const MOD_NAME As String = "SPA II"
    Private _initialized As Boolean = False
    Private _isEnhanced As Boolean = False

    ''' <summary>
    ''' Gets whether the game is running on GTA V Enhanced Edition.
    ''' SHVDNE bumps FileVersion major by 1 for Enhanced builds.
    ''' </summary>
    Public ReadOnly Property IsEnhanced As Boolean
        Get
            If Not _initialized Then Initialize()
            Return _isEnhanced
        End Get
    End Property

    ''' <summary>
    ''' Gets the base path for mod data (config, saves, garages).
    ''' On Enhanced: %localappdata%\SPA II\
    ''' On Legacy: .\scripts\SPA II\
    ''' </summary>
    Public ReadOnly Property ModDataPath As String
        Get
            If Not _initialized Then Initialize()
            If _isEnhanced Then
                Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MOD_NAME)
            Else
                Return Path.Combine(".", "scripts", MOD_NAME)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the path for garage vehicle XML files.
    ''' </summary>
    Public ReadOnly Property GaragePath As String
        Get
            Return Path.Combine(ModDataPath, "Garages")
        End Get
    End Property

    ''' <summary>
    ''' Gets the path for sound files.
    ''' Sound files are read-only assets and can stay in scripts folder.
    ''' </summary>
    Public ReadOnly Property SoundPath As String
        Get
            Return Path.Combine(".", "scripts", MOD_NAME, "Sounds")
        End Get
    End Property

    ''' <summary>
    ''' Gets the path for the mod config file.
    ''' </summary>
    Public ReadOnly Property ConfigFilePath As String
        Get
            Return Path.Combine(ModDataPath, "modconfig.ini")
        End Get
    End Property

    ''' <summary>
    ''' Gets the path for the log file.
    ''' </summary>
    Public ReadOnly Property LogFilePath As String
        Get
            If Not _initialized Then Initialize()
            If _isEnhanced Then
                Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MOD_NAME, "SPA II.log")
            Else
                Return ".\SPA II.log"
            End If
        End Get
    End Property

    ''' <summary>
    ''' Gets the path for the debug log file.
    ''' </summary>
    Public ReadOnly Property DebugLogFilePath As String
        Get
            If Not _initialized Then Initialize()
            If _isEnhanced Then
                Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MOD_NAME, "SPA II.txt")
            Else
                Return ".\SPA II.txt"
            End If
        End Get
    End Property

    ''' <summary>
    ''' Initializes the path configuration and detects Enhanced Edition.
    ''' </summary>
    Public Sub Initialize()
        If _initialized Then Return

        ' Detect Enhanced Edition
        ' SHVDNE uses Game.Version >= v1_0_3095_0 for Enhanced detection
        ' Enhanced version numbers start around 3095+
        ' We also check for version numbers > 100 as a safety margin
        Try
            Dim version As Integer = CInt(Game.Version)
            ' Enhanced builds have version numbers >= 65 (v1_0_3095_0 enum value in SHVDNE)
            ' Legacy builds max out around 63-64
            _isEnhanced = version >= 65
        Catch
            ' If we can't determine version, assume Legacy for safety
            _isEnhanced = False
        End Try

        _initialized = True

        ' Ensure directories exist for Enhanced
        If _isEnhanced Then
            EnsureDirectoriesExist()
        End If
    End Sub

    ''' <summary>
    ''' Ensures all required directories exist.
    ''' </summary>
    Public Sub EnsureDirectoriesExist()
        Try
            If Not Directory.Exists(ModDataPath) Then
                Directory.CreateDirectory(ModDataPath)
            End If
            If Not Directory.Exists(GaragePath) Then
                Directory.CreateDirectory(GaragePath)
            End If
        Catch ex As Exception
            ' Silently fail - will be caught when actually trying to write
        End Try
    End Sub

    ''' <summary>
    ''' Migrates data from Legacy paths to Enhanced paths if needed.
    ''' Call this once during mod initialization.
    ''' </summary>
    Public Sub MigrateDataIfNeeded()
        If Not _isEnhanced Then Return

        Try
            Dim legacyConfigPath As String = Path.Combine(".", "scripts", MOD_NAME, "modconfig.ini")
            Dim legacyGaragePath As String = Path.Combine(".", "scripts", MOD_NAME, "Garages")

            ' Migrate config file
            If File.Exists(legacyConfigPath) AndAlso Not File.Exists(ConfigFilePath) Then
                EnsureDirectoriesExist()
                File.Copy(legacyConfigPath, ConfigFilePath, False)
            End If

            ' Migrate garage files
            If Directory.Exists(legacyGaragePath) AndAlso Not Directory.Exists(GaragePath) Then
                EnsureDirectoriesExist()
                CopyDirectory(legacyGaragePath, GaragePath)
            ElseIf Directory.Exists(legacyGaragePath) AndAlso Directory.Exists(GaragePath) Then
                ' If both exist, copy any missing files
                For Each dir As String In Directory.GetDirectories(legacyGaragePath)
                    Dim dirName As String = Path.GetFileName(dir)
                    Dim targetDir As String = Path.Combine(GaragePath, dirName)
                    If Not Directory.Exists(targetDir) Then
                        CopyDirectory(dir, targetDir)
                    End If
                Next
            End If
        Catch ex As Exception
            ' Migration failed - user may need to manually copy files
            Logger.Log($"Migration failed: {ex.Message}")
        End Try
    End Sub

    Private Sub CopyDirectory(sourceDir As String, destDir As String)
        If Not Directory.Exists(destDir) Then
            Directory.CreateDirectory(destDir)
        End If

        For Each file As String In Directory.GetFiles(sourceDir)
            Dim destFile As String = Path.Combine(destDir, Path.GetFileName(file))
            If Not IO.File.Exists(destFile) Then
                IO.File.Copy(file, destFile)
            End If
        Next

        For Each subDir As String In Directory.GetDirectories(sourceDir)
            Dim destSubDir As String = Path.Combine(destDir, Path.GetFileName(subDir))
            CopyDirectory(subDir, destSubDir)
        Next
    End Sub

End Module
