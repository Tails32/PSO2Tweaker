﻿Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Diagnostics
Imports DevComponents.DotNetBar

Public Class frmOptions
    Private Declare Auto Function ShellExecute Lib "shell32.dll" (ByVal hwnd As IntPtr, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As UInteger) As IntPtr

    Private Sub frmOptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SetLocale()
        Me.CMBStyle.SelectedIndex = -1
    End Sub

    Private Function GetBackupMode(ByRef Key As String) As String
        Dim Value As String = RegKey.GetValue(Of String)(Key)
        Select Case Value
            Case "Ask"
                Return "Ask every time"
            Case "Always"
                Return "Always backup"
            Case "Never"
                Return "Never backup"
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub frmOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.SuspendLayout()
            If Not String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.Color)) Then ColorPickerButton1.SelectedColor = Color.FromArgb(RegKey.GetValue(Of Integer)(RegKey.Color))
            If Not String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.FontColor)) Then ColorPickerButton2.SelectedColor = Color.FromArgb(RegKey.GetValue(Of Integer)(RegKey.FontColor))
            If Not String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.TextBoxBGColor)) Then ColorPickerButton4.SelectedColor = Color.FromArgb(RegKey.GetValue(Of Integer)(RegKey.TextBoxBGColor))
            If Not String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.TextBoxColor)) Then ColorPickerButton3.SelectedColor = Color.FromArgb(RegKey.GetValue(Of Integer)(RegKey.TextBoxColor))

            Dim BackupMode = GetBackupMode(RegKey.Backup)

            If Not String.IsNullOrEmpty(BackupMode) Then
                cmbBackupPreference.Text = BackupMode
            End If

            BackupMode = GetBackupMode(RegKey.PreDownloadedRAR)

            If Not String.IsNullOrEmpty(BackupMode) Then
                cmbPredownload.Text = BackupMode
            End If

            ' Checks if the DPI greater or equal to 120, and sets accordingly.
            ' Otherwise, we'll assume is 96 or lower.
            Using g As Graphics = Me.CreateGraphics
                If g.DpiX >= 120 Then
                    Me.Size = New Size(583, 554)
                Else
                    Me.Size = New Size(440, 451)
                End If
            End Using

            ' Here we pull the locale setting from registry and apply it to the form.
            ' Reads locale from registry and converts from LangCode (e.g "en") to Language (e.g "English")
            Try
                Dim Locale As Language = DirectCast([Enum].Parse(GetType(LangCode), RegKey.GetValue(Of String)(RegKey.Locale)), Language)

                cmbLanguage.Text = Locale.ToString()
                Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(CType(Locale, LangCode).ToString())
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
            Catch ex As Exception
                cmbLanguage.Text = "English"
                Thread.CurrentThread.CurrentUICulture = Helper.DefaltCultureInfo
                Thread.CurrentThread.CurrentCulture = Helper.DefaltCultureInfo
            End Try

            LabelX1.Text = My.Resources.strChooseATheme
            LabelX2.Text = My.Resources.strChooseALanguage
            LabelX3.Text = My.Resources.strChooseABackgroundImage

            CheckBoxX1.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.Pastebin))
            CheckBoxX2.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.ENPatchAfterInstall))
            CheckBoxX3.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.LargeFilesAfterInstall))
            CheckBoxX4.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.StoryPatchAfterInstall))
            CheckBoxX5.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.SidebarEnabled))

            chkAutoRemoveCensor.Checked = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.RemoveCensor))
            CMBStyle.Text = RegKey.GetValue(Of String)(RegKey.Style)

            ComboItem33.Text = "Last installed: " & RegKey.GetValue(Of String)(RegKey.StoryPatchVersion)
            ComboItem33.Text = "Latest version: " & RegKey.GetValue(Of String)(RegKey.NewStoryVersion)
            ComboItem35.Text = "Last installed: " & RegKey.GetValue(Of String)(RegKey.ENPatchVersion)
            ComboItem36.Text = "Latest version: " & RegKey.GetValue(Of String)(RegKey.NewENVersion)
            ComboItem40.Text = "Last installed: " & RegKey.GetValue(Of String)(RegKey.LargeFilesVersion)
            ComboItem42.Text = "Latest version: " & RegKey.GetValue(Of String)(RegKey.NewLargeFilesVersion)
        Catch ex As Exception
            frmMain.Log(ex.Message)
            frmMain.WriteDebugInfo(My.Resources.strERROR & ex.Message)
        Finally
            Me.ResumeLayout(False)
        End Try
    End Sub

    Private Sub CMBStyle_SelectedValueChanged(sender As Object, e As EventArgs) Handles CMBStyle.SelectedValueChanged
        If Not String.IsNullOrEmpty(CMBStyle.Text) Then

            '┻━┻ ︵ \(Ò_Ó \)
            '(╯°□°）╯︵ /(.□. \)
            '┯━┯ノ(º₋ºノ)

            Select Case CMBStyle.Text
                Case "Blue"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2007Blue
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case "Silver"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2007Silver
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case "Black"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2007Black
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case "Vista Glass"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2007VistaGlass
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case "2010 Silver"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2010Silver
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case "Windows 7 Blue"
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Windows7Blue
                    RegKey.SetValue(Of String)(RegKey.Style, CMBStyle.Text)

                Case Else
                    StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2007Blue
                    RegKey.SetValue(Of String)(RegKey.Style, "Blue")
            End Select
        End If
    End Sub

    Private Sub ComboBoxEx1_SelectedIndexChanged(sender As Object, e As EventArgs)
        RegKey.SetValue(Of String)(RegKey.PatchServer, ComboBoxEx1.Text)
    End Sub

    Private Sub cmbLanguage_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbLanguage.SelectedValueChanged
        Dim DownloadClient As New WebClient
        DownloadClient.DownloadFile(New Uri("http://162.243.211.123/freedom/LanguagePack.rar"), "LanguagePack.rar")

        Dim processStartInfo = New ProcessStartInfo()
        processStartInfo.FileName = (Application.StartupPath & "\unrar.exe").Replace("\\", "\")
        processStartInfo.Verb = "runas"
        processStartInfo.Arguments = "x -inul -o+ LanguagePack.rar"
        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden
        processStartInfo.UseShellExecute = True

        Dim extractorProcess = Process.Start(processStartInfo)

        Do Until extractorProcess.WaitForExit(1000)
        Loop

        SetLocale()
    End Sub

    Private Sub SetLocale()
        Dim SelectedLocale As String = [Enum].GetName(GetType(LangCode), cmbLanguage.SelectedIndex)

        If String.IsNullOrEmpty(SelectedLocale) Then
            Thread.CurrentThread.CurrentUICulture = Helper.DefaltCultureInfo
            Thread.CurrentThread.CurrentCulture = Helper.DefaltCultureInfo
            RegKey.SetValue(Of String)(RegKey.Locale, "en")
        Else
            Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(SelectedLocale)
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
            RegKey.SetValue(Of String)(RegKey.Locale, SelectedLocale)
        End If
    End Sub

    Private Sub ColorPickerButton1_SelectedColorChanged(sender As Object, e As EventArgs) Handles ColorPickerButton1.SelectedColorChanged
        StyleManager.ColorTint = ColorPickerButton1.SelectedColor
        RegKey.SetValue(Of Integer)(RegKey.Color, (ColorPickerButton1.SelectedColor.ToArgb))
    End Sub

    Private Sub CheckBoxX1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxX1.CheckedChanged
        If CheckBoxX1.Checked = False Then
            MsgBox("PLEASE BE CAUTIOUS - If you turn this function off, the program will not automatically upload your logfile to pastebin, so you can report the bug to AIDA. This means that you'll need to provide the logfile yourself, or the likelyhood of your issue being resolved is very, very, slim.")
        End If

        RegKey.SetValue(Of String)(RegKey.Pastebin, CheckBoxX1.Checked.ToString())
    End Sub

    Private Sub ColorPickerButton2_SelectedColorChanged(sender As Object, e As EventArgs) Handles ColorPickerButton2.SelectedColorChanged
        frmMain.ForeColor = ColorPickerButton2.SelectedColor
        frmPSO2Options.ForeColor = ColorPickerButton2.SelectedColor
        frmPSO2Options.TabItem1.TextColor = ColorPickerButton2.SelectedColor
        frmPSO2Options.TabItem2.TextColor = ColorPickerButton2.SelectedColor
        frmPSO2Options.TabItem3.TextColor = ColorPickerButton2.SelectedColor
        Me.ForeColor = ColorPickerButton2.SelectedColor
        CheckBoxX1.TextColor = ColorPickerButton2.SelectedColor
        CheckBoxX2.TextColor = ColorPickerButton2.SelectedColor
        CheckBoxX3.TextColor = ColorPickerButton2.SelectedColor
        CheckBoxX4.TextColor = ColorPickerButton2.SelectedColor
        CheckBoxX5.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRemoveCensor.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRemoveNVidia.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRemovePC.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRemoveSEGA.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRemoveVita.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRestoreCensor.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRestoreNVidia.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRestorePC.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRestoreSEGA.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkRestoreVita.TextColor = ColorPickerButton2.SelectedColor
        frmMain.chkSwapOP.TextColor = ColorPickerButton2.SelectedColor

        RegKey.SetValue(Of Integer)(RegKey.FontColor, (ColorPickerButton2.SelectedColor.ToArgb))
    End Sub

    Private Sub CheckBoxX2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxX2.CheckedChanged
        RegKey.SetValue(Of String)(RegKey.ENPatchAfterInstall, CheckBoxX2.Checked.ToString())
    End Sub

    Private Sub CheckBoxX3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxX3.CheckedChanged
        RegKey.SetValue(Of String)(RegKey.LargeFilesAfterInstall, CheckBoxX3.Checked.ToString())
    End Sub

    Private Sub CheckBoxX4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxX4.CheckedChanged
        RegKey.SetValue(Of String)(RegKey.StoryPatchAfterInstall, CheckBoxX4.Checked.ToString())
    End Sub

    Private Sub cmbBackupPreference_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBackupPreference.SelectedIndexChanged
        Select Case cmbBackupPreference.SelectedIndex
            Case 0
                RegKey.SetValue(Of String)(RegKey.Backup, "Ask")

            Case 1
                RegKey.SetValue(Of String)(RegKey.Backup, "Always")

            Case 2
                RegKey.SetValue(Of String)(RegKey.Backup, "Never")

            Case Else
                RegKey.SetValue(Of String)(RegKey.Backup, "Ask")
        End Select
    End Sub

    Private Sub ButtonX1_Click(sender As Object, e As EventArgs) Handles ButtonX1.Click
        Process.Start("http://arks-layer.com/credits.php")
    End Sub

    Private Sub UpdateVersion(key As String, str As String)
        Dim value As String = str.Replace("Latest version: ", "").Replace("Last installed: ", "")
        RegKey.SetValue(Of String)(key, value)
        MsgBox(value)
    End Sub

    Private Sub cmbENOverride_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbENOverride.SelectedIndexChanged
        UpdateVersion(RegKey.ENPatchVersion, cmbENOverride.Text)
    End Sub

    Private Sub cmbLargeFilesOverride_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLargeFilesOverride.SelectedIndexChanged
        UpdateVersion(RegKey.LargeFilesVersion, cmbLargeFilesOverride.Text)
    End Sub

    Private Sub cmbStoryOverride_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStoryOverride.SelectedIndexChanged
        UpdateVersion(RegKey.StoryPatchVersion, cmbStoryOverride.Text)
    End Sub

    Private Sub ColorPickerButton4_SelectedColorChanged(sender As Object, e As EventArgs) Handles ColorPickerButton4.SelectedColorChanged
        frmMain.rtbDebug.BackColor = ColorPickerButton4.SelectedColor
        RegKey.SetValue(Of Integer)(RegKey.TextBoxBGColor, (ColorPickerButton4.SelectedColor.ToArgb))
    End Sub

    Private Sub ColorPickerButton3_SelectedColorChanged(sender As Object, e As EventArgs) Handles ColorPickerButton3.SelectedColorChanged
        frmMain.rtbDebug.ForeColor = ColorPickerButton3.SelectedColor
        RegKey.SetValue(Of Integer)(RegKey.TextBoxColor, (ColorPickerButton3.SelectedColor.ToArgb))
    End Sub

    Private Sub btnPSO2Override_Click(sender As Object, e As EventArgs) Handles btnPSO2Override.Click
        Dim YesNo As MsgBoxResult = MsgBox("This will tell the Tweaker you have the latest version of PSO2 installed - Be aware that this cannot be undone, and should only be used if you update the game outside of the Tweaker. Do you want to continue?", vbYesNo)

        If YesNo = vbYes Then
            Dim lines3 = File.ReadAllLines("version.ver")
            Dim RemoteVersion3 As String = lines3(0)
            RegKey.SetValue(Of String)(RegKey.PSO2RemoteVersion, RemoteVersion3)
            MsgBox("PSO2 Installed version set to: " & RemoteVersion3)
        End If
    End Sub

    Private Sub ButtonX3_Click(sender As Object, e As EventArgs)
        If Not String.IsNullOrWhiteSpace(TextBoxX1.Text) Then
            Dim UIDString As String = TextBoxX1.Text.Replace("steam://rungameid/", "")
            RegKey.SetValue(Of String)(RegKey.SteamUID, UIDString)
            MsgBox(UIDString)
        End If
    End Sub

    Private Sub ButtonX5_Click(sender As Object, e As EventArgs)
        Environment.SetEnvironmentVariable("-pso2", "+0x01e3f1e9")
        ShellExecute(Handle, "open", ("steam://rungameID/" & RegKey.GetValue(Of String)(RegKey.SteamUID)), " +0x33aca2b9 -pso2", "", 0)
    End Sub

    Private Sub CheckBoxX5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxX5.CheckedChanged
        RegKey.SetValue(Of String)(RegKey.SidebarEnabled, CheckBoxX5.Checked.ToString())
    End Sub

    Private Sub chkAutoRemoveCensor_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoRemoveCensor.CheckedChanged
        RegKey.SetValue(Of String)(RegKey.RemoveCensor, chkAutoRemoveCensor.Checked.ToString())
    End Sub

    Private Sub cmbPredownload_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPredownload.SelectedIndexChanged
        If cmbPredownload.SelectedIndex = 0 Then
            RegKey.SetValue(Of String)(RegKey.PreDownloadedRAR, "Ask")
        ElseIf cmbPredownload.SelectedIndex = 1 Then
            RegKey.SetValue(Of String)(RegKey.PreDownloadedRAR, "Never")
        End If
    End Sub
End Class
