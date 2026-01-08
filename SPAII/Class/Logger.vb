Imports System.IO

''' <summary>
''' Logger class that writes to LocalAppData on Enhanced Edition
''' and to game folder on Legacy Edition for compatibility.
''' </summary>
Public NotInheritable Class Logger

    Private Sub New()
    End Sub

    Public Shared Sub Log(message As Object)
        Try
            EnsureLogDirectoryExists()
            IO.File.AppendAllText(PathConfig.LogFilePath, DateTime.Now & ":" & message & Environment.NewLine)
        Catch
            ' Silently fail if we can't write logs
        End Try
    End Sub

    Public Shared Sub Logg(message As Object)
        Try
            EnsureLogDirectoryExists()
            IO.File.AppendAllText(PathConfig.DebugLogFilePath, message & Environment.NewLine)
        Catch
            ' Silently fail if we can't write logs
        End Try
    End Sub

    Private Shared Sub EnsureLogDirectoryExists()
        If PathConfig.IsEnhanced Then
            Dim logDir As String = Path.GetDirectoryName(PathConfig.LogFilePath)
            If Not Directory.Exists(logDir) Then
                Directory.CreateDirectory(logDir)
            End If
        End If
    End Sub

End Class