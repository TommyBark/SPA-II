Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports GTA.UI
Imports System.Runtime.CompilerServices

''' <summary>
''' Compatibility layer for migrating from ScriptHookVDotNet2 to ScriptHookVDotNet3.
''' Provides wrapper functions for deprecated/changed APIs.
''' </summary>
Module SHVDN3Compat

    ''' <summary>
    ''' Gets a localized text entry (replacement for GetGXTEntry).
    ''' In SHVDN3, use GTA.Game.GetLocalizedString or native calls.
    ''' </summary>
    Public Function GetGXTEntry(entry As String) As String
        Return Game.GetLocalizedString(entry)
    End Function

    ''' <summary>
    ''' Gets the friendly/localized name of a vehicle.
    ''' In SHVDN3, Vehicle.LocalizedName was renamed to LocalizedName.
    ''' </summary>
    <Extension>
    Public Function GetFriendlyName(vehicle As Vehicle) As String
        Return vehicle.LocalizedName
    End Function

    ''' <summary>
    ''' Gets the blip attached to a vehicle.
    ''' In SHVDN3, use AttachedBlip instead of CurrentBlip.
    ''' </summary>
    <Extension>
    Public Function GetCurrentBlip(entity As Entity) As Blip
        Return entity.AttachedBlip
    End Function

    ''' <summary>
    ''' Gets a decorator integer value from an entity.
    ''' In SHVDN3, use Decor.GetInt directly.
    ''' </summary>
    <Extension>
    Public Function GetDecorInt(entity As Entity, decorName As String) As Integer
        Return Decor.GetInt(entity, decorName)
    End Function

    ''' <summary>
    ''' Sets a decorator integer value on an entity.
    ''' In SHVDN3, use Decor.SetInt directly.
    ''' </summary>
    <Extension>
    Public Sub SetDecorInt(entity As Entity, decorName As String, value As Integer)
        Decor.Set(entity, decorName, value)
    End Sub

    ''' <summary>
    ''' Shows a help message on screen.
    ''' In SHVDN3, ShowHelpMessage was moved to Screen namespace.
    ''' </summary>
    Public Sub ShowHelpMessage(message As String, Optional duration As Integer = -1, Optional beep As Boolean = True, Optional looped As Boolean = False)
        Screen.ShowHelpText(message, duration, beep, looped)
    End Sub

    ''' <summary>
    ''' Shows a subtitle on screen.
    ''' In SHVDN3, ShowSubtitle was moved to Screen namespace.
    ''' </summary>
    Public Sub ShowSubtitle(message As String, Optional duration As Integer = 2500)
        Screen.ShowSubtitle(message, duration)
    End Sub

    ''' <summary>
    ''' Creates a camera (wrapper for deprecated World.CreateCamera).
    ''' In SHVDN3, use Camera.Create instead.
    ''' </summary>
    Public Function CreateCamera(position As Vector3, rotation As Vector3, fov As Single) As Camera
        Dim cam As Camera = World.CreateCamera(position, rotation, fov)
        Return cam
    End Function

    ''' <summary>
    ''' Destroys all cameras (wrapper for deprecated World.DestroyAllCameras).
    ''' In SHVDN3, use Camera.DeleteAllCameras instead.
    ''' </summary>
    Public Sub DestroyAllCameras()
        Camera.DeleteAllCameras()
    End Sub

End Module
