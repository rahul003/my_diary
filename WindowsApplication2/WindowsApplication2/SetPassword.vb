'This form allows a new user to set a password for the My Diary software

Imports System.IO
Imports System
Imports System.Security
Imports System.Security.Cryptography 'For salting the password
Imports System.Text


Public Class SetPassword 'this form sets a password
    Dim passwordHash As String
    Dim FILE_NAME As String = "output/pass.p"  'this is where the password is stored

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click 'on clicking set password button
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, False)
        Dim strText As String = TextBox1.Text 'the text to be hashed, i.e the password
        Dim salt As String = "1131042761328" 'additional input to hash function
        Dim bytHashedData As Byte()
        Dim encoder As New UTF8Encoding()
        Dim md5Hasher As New MD5CryptoServiceProvider

        ' Get Bytes for "password"
        Dim passwordBytes As Byte() = encoder.GetBytes(strText)

        ' Get Bytes for "salt"
        Dim saltBytes As Byte() = encoder.GetBytes(salt)

        ' Creat new Array to store both "password" and "salt" bytes
        Dim passwordAndSaltBytes As Byte() = _
        New Byte(passwordBytes.Length + saltBytes.Length - 1) {}

        ' Store "password" bytes
        For i As Integer = 0 To passwordBytes.Length - 1
            passwordAndSaltBytes(i) = passwordBytes(i)
        Next

        ' Append "salt" bytes
        For i As Integer = 0 To saltBytes.Length - 1
            passwordAndSaltBytes(i + passwordBytes.Length) = saltBytes(i)
        Next

        ' Compute hash value for "password" and "salt" bytes
        bytHashedData = md5Hasher.ComputeHash(passwordAndSaltBytes)

        ' Convert result into a base64-encoded string.
        Dim hashValue As String
        hashValue = Convert.ToBase64String(bytHashedData)

        MessageBox.Show("Password has been set successfully. Now Click Ok to go to the Unlock screen")
        objWriter.WriteLine(hashValue) 'write the hashedpasswordvalue to the file
        objWriter.Close()

        'show lockscreen now by calling an object
        Dim locker As Lockscreen = New Lockscreen
        locker.Show()
        Me.Hide()
    End Sub

    Private Sub SetPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'on load of form, if output folder doesnt exist, creat it
        If (Not System.IO.Directory.Exists("output")) Then
            System.IO.Directory.CreateDirectory("output")
        End If
       
    End Sub
    Private Sub frmProgramma_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Lockscreen.Dispose()
    End Sub
End Class
