#NoEnv
SendMode Input
SetWorkingDir %A_ScriptDir%

targetProcessPath := "C:\Users\jrafa\AppData\Local\Programs\Microsoft VS Code\Code.exe"

Loop
{
    WinGet, targetWindow, ID, ahk_exe %targetProcessPath%

    if (targetWindow)
    {
        WinGetPos, WinX, WinY, WinWidth, WinHeight, A
        NewWidth := (A_ScreenWidth * 2) / 3
        NewHeight := A_ScreenHeight
        WinMove, % "ahk_id " targetWindow, , A_ScreenWidth / 3, 0, NewWidth, NewHeight
    }

    Sleep 1000 ; Wait for 1 second before checking again
}
