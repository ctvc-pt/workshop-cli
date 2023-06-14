#NoEnv
SendMode Input
SetWorkingDir %A_ScriptDir%

; Move the active window to two-thirds of the screen on the right when the script runs
WinGetPos, WinX, WinY, WinWidth, WinHeight, A
NewWidth := (A_ScreenWidth * 2) / 3
NewHeight := A_ScreenHeight
WinMove, A,, A_ScreenWidth / 3, 0, NewWidth, NewHeight

ExitApp ; Exit the script after moving the window
