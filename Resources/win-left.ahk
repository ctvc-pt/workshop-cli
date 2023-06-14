#NoEnv
SendMode Input
SetWorkingDir %A_ScriptDir%

; Move the active window to one-third of the screen on the left when the script runs
WinGetPos, WinX, WinY, WinWidth, WinHeight, A
NewWidth := A_ScreenWidth / 3
NewHeight := A_ScreenHeight
WinMove, A,, 0, 0, NewWidth, NewHeight



ExitApp  ; Exit the script after moving and maximizing the window
