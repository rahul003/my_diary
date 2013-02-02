'This is the main interface form of the Diary software. Here the user can view the various entries that he/she has created.
'There are links to all other features of the program

Imports System.IO

Public Class Main
    Public newfile As String
    Public Const sSecretKey As String = "Password" 'sSecretKey is the string used to encrypt file

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'create a form object to create entries
        Dim oForm As Create = New Create
        oForm.Show()
    End Sub
    Private Sub frmProgramma_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        'on clikcing close button, show message box
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            'delete .txts if any in the folder
            Dim di As New IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory() + "/output")
            Dim diar1 As IO.FileInfo() = di.GetFiles("*.txt")
            Dim dra As IO.FileInfo
            For Each dra In diar1
                My.Computer.FileSystem.DeleteFile("output/" + dra.Name)
            Next
            'close the parent of all
            Lockscreen.Close()
        Else
            'cancel
            e.Cancel = True
        End If
    End Sub

    Private Sub Main_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load all entries into listview
        RefreshList()
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        'identify the item
        Dim lvi As ListViewItem = ListView1.HitTest(e.Location).Item
        'if some item selected
        If lvi IsNot Nothing Then
            'open edit form
            Dim oForm As Edit = New Edit
            oForm.Show()

            Dim contents As String
            Dim title As String
            Dim thedate As String
            Dim time As String

            'decrypt the file to create txt
            Dim decrypt As File_Crypt = New File_Crypt
            newfile = "output/" + lvi.Text + ".txt"
            decrypt.DecryptFile("output/" + lvi.Text + ".encrypted", newfile, sSecretKey)

            'read from .txt and store it into these variables
            Using sr As New StreamReader(newfile)
                thedate = sr.ReadLine()
                time = sr.ReadLine()
                title = sr.ReadLine()
                contents = sr.ReadToEnd()
            End Using

            'set the contents of the edit form object to these variables
            oForm.DateTimePicker2.Text = thedate
            oForm.initial = thedate + ".encrypted"
            oForm.TimeBox.Text = time
            oForm.TextBox2.Text = title
            oForm.TextBox3.Text = contents

        End If
    End Sub

    Public Sub RefreshList()
        'find all .encrypted files in the directory
        Dim di As New IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory() + "/output")
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.encrypted")
        Dim dra As IO.FileInfo
        Dim itmx As New ListViewItem
        Dim tempfile As String
        Dim decryptingtemp As File_Crypt = New File_Crypt
        'list the names of all files in the specified directory
        For Each dra In diar1
            'add title
            itmx = ListView1.Items.Add(dra.Name.Substring(0, dra.Name.Length - 10))

            'name for tempfile is name substring + .txt
            tempfile = "output/" + dra.Name.Substring(0, dra.Name.Length - 9) + "txt"

            'decrypt the files and read the title and date to ListView
            decryptingtemp.DecryptFile("output/" + dra.Name, tempfile, sSecretKey)
            Using reader As New StreamReader(tempfile)
                reader.ReadLine() 'since i dont require 1st and 2nd lines, i dont use it
                reader.ReadLine()
                itmx.SubItems.Add(reader.ReadLine()) 'add the title from 3rd line
            End Using
            'delete the txt temporary files
            My.Computer.FileSystem.DeleteFile(tempfile)
        Next

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For Each i As ListViewItem In ListView1.SelectedItems
            ListView1.Items.Remove(i)
            My.Computer.FileSystem.DeleteFile("output/" + i.Text + ".encrypted")
        Next
    End Sub
   

    Private Sub ExitToolStripMenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem7.Click
        If MessageBox.Show("Are you sure to close this application?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Lockscreen.Close()
        Else

        End If
    End Sub

    Private Sub ChangePasswordToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangePasswordToolStripMenuItem4.Click
        Dim changepassword As change = New change
        changepassword.Show()
    End Sub

    Private Sub AboutToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem5.Click
        MessageBox.Show("This Software has been developed by Rahul Huilgol as part of an assignment for the course Software Engineering. Open Readme file [Link Available in the Help Section] for help on How to Use the software", "About")

    End Sub

    Private Sub ReadMeToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadMeToolStripMenuItem4.Click
        System.Diagnostics.Process.Start("Notepad.Exe", "ReadMe.txt")
    End Sub
End Class
