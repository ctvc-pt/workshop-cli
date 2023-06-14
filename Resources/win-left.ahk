#NoEnv
SendMode Input
SetWorkingDir %A_ScriptDir%

; Get the screen width and height
SysGet, MonitorWorkArea, MonitorWorkArea
ScreenWidth := MonitorWorkAreaRight - MonitorWorkAreaLeft
ScreenHeight := MonitorWorkAreaBottom - MonitorWorkAreaTop

; Calculate the new window dimensions
NewWidth := ScreenWidth / 3
NewHeight := ScreenHeight

; Move the active window to the left and maximize the height
WinGetPos, WinX, WinY, WinWidth, WinHeight, A
NewX := MonitorWorkAreaLeft
NewY := MonitorWorkAreaTop
WinMove, A,, NewX, NewY, NewWidth, NewHeight

ExitApp ; Exit the script after moving and maximizing the window
