'Edit is a form in which we can change an existing entry

Imports System.Text
Imports System.IO

Public Class Edit
    Public initial As String 'this initial has been set from Diary.vb
    Dim final, initialcut As String
    Private Sub frmProgramma_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'on trying to close form, delete if any .txt remain in the folder
        Dim di As New IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory() + "/output")
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.txt")
        Dim dra As IO.FileInfo
        For Each dra In diar1
            My.Computer.FileSystem.DeleteFile("output/" + dra.Name)
        Next
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        final = DateTimePicker2.Text + ".txt"
        'final is the name of the edited new txt

        'initialcut is the substring of name without the extension
        initialcut = initial.Substring(0, initial.Length - 9)

        'delete initial.encrypted
        My.Computer.FileSystem.DeleteFile(System.AppDomain.CurrentDomain.BaseDirectory() + "output/" + initial)

        'create and write to .txt
        Dim objWriter As New System.IO.StreamWriter("output/" + DateTimePicker2.Text + ".txt", False)
        objWriter.WriteLine(DateTimePicker2.Text)
        objWriter.WriteLine(DateTimePicker2.Value.ToLongTimeString()) 'write time to file
        objWriter.WriteLine(TextBox2.Text)
        objWriter.Write(TextBox3.Text)
        objWriter.Close()
        MsgBox("Entry has been Saved")

        'now encrypt .txt to create .encrypted
        Dim crypt As File_Crypt = New File_Crypt
        crypt.EncryptFile("output/" + DateTimePicker2.Text + ".txt", "output/" + DateTimePicker2.Text + ".encrypted", Main.sSecretKey)

        'Clear the items list
        Main.ListView1.Items.Clear()

        'reload the list to add new file
        Main.RefreshList()
        Me.Close()
    End Sub

End Class