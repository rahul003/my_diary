'This form is used to change password

Imports System.IO
Imports System
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Public Class change 'this form is used to change the password
    Dim passwordhash As String
    Dim FILE_PATH As String = "output/pass.p"

    Private Sub ChangeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeButton.Click

        Using reader As StreamReader = New StreamReader(FILE_PATH, False)

            passwordhash = reader.ReadLine() 'this fetches the passwordhash saved in pass.p
        End Using

        'the next few lines hash the textbox text to check it with current password
        Dim strText As String = TextBox1.Text
        Dim salt As String = "1131042761328"
        Dim bytHashedData As Byte()
        Dim encoder As New UTF8Encoding()
        Dim md5Hasher As New MD5CryptoServiceProvider
        Dim passwordBytes As Byte() = encoder.GetBytes(strText)
        Dim saltBytes As Byte() = encoder.GetBytes(salt)
        Dim passwordAndSaltBytes As Byte() = _
        New Byte(passwordBytes.Length + saltBytes.Length - 1) {}
        For i As Integer = 0 To passwordBytes.Length - 1
            passwordAndSaltBytes(i) = passwordBytes(i)
        Next
        For i As Integer = 0 To saltBytes.Length - 1
            passwordAndSaltBytes(i + passwordBytes.Length) = saltBytes(i)
        Next
        bytHashedData = md5Hasher.ComputeHash(passwordAndSaltBytes)
        Dim hashValue As String
        hashValue = Convert.ToBase64String(bytHashedData)


        'check with current passwordhash. if equal proceed to change
        If hashValue = passwordhash Then

            'similarly hash the newpassword textbox input
            Dim newpassword As String = TextBox2.Text

            passwordBytes = encoder.GetBytes(newpassword)
            saltBytes = encoder.GetBytes(salt)
            passwordAndSaltBytes = _
            New Byte(passwordBytes.Length + saltBytes.Length - 1) {}
            For i As Integer = 0 To passwordBytes.Length - 1
                passwordAndSaltBytes(i) = passwordBytes(i)
            Next
            For i As Integer = 0 To saltBytes.Length - 1
                passwordAndSaltBytes(i + passwordBytes.Length) = saltBytes(i)
            Next
            bytHashedData = md5Hasher.ComputeHash(passwordAndSaltBytes)
            hashValue = Convert.ToBase64String(bytHashedData)
            Dim objWriter As New System.IO.StreamWriter(FILE_PATH, False)
            'writes present hashvalue to file
            objWriter.WriteLine(hashValue)
            objWriter.Close()
            MessageBox.Show("Password successfully changed")
        Else
            MessageBox.Show("Please enter correct present password")
        End If
        Me.Close()
    End Sub

End Class
