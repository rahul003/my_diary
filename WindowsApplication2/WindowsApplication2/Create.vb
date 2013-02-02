'This is the form which allows user to create entry

Imports System.Text
Imports System.IO

Public Class Create
    '64 or 8bits
    Private Const sSecretKey As String = "Password"
    Public FILE_NAME As String
    Private isExist As Boolean = False 'this variable helps us see if a file with that name alredy exists
   
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click 'on clicking save

        isExist = False 'initializing
        'checks if file with that name already exists
        Dim di As New IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory() + "output")
        Dim allentries As IO.FileInfo() = di.GetFiles("*.encrypted")
        Dim dra As IO.FileInfo
        For Each dra In allentries
            If (DateTimePicker2.Text = dra.Name.Substring(0, dra.Name.Length - 10)) Then
                isExist = True
            End If
        Next

        'if it exits then show message
        If isExist Then
            MessageBox.Show("There is already an entry corresponding to this date. Please edit the corresponding entry. Multiple entries on same date are not accepted")

            'if not continue

        Else
            If (Not System.IO.Directory.Exists("output")) Then
                System.IO.Directory.CreateDirectory("output")
            End If
            Dim objWriter As New System.IO.StreamWriter("output/" + DateTimePicker2.Text + ".txt", False)
            'write all contents of boxes to file
            objWriter.WriteLine(DateTimePicker2.Text)
            objWriter.WriteLine(DateTimePicker2.Value.ToLongTimeString())
            objWriter.WriteLine(TextBox2.Text)
            objWriter.Write(TextBox3.Text)
            objWriter.Dispose()

            'now encrypt that .txt
            Dim crypt As File_Crypt = New File_Crypt
            crypt.EncryptFile("output/" + DateTimePicker2.Text + ".txt", "output/" + DateTimePicker2.Text + ".encrypted", sSecretKey)
            Me.Close()
            MsgBox("Entry has been Saved")

            'clear listview
            Main.ListView1.Items.Clear()

            'add listview items
            Main.RefreshList()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'cancel button
        Me.Dispose()
    End Sub
End Class