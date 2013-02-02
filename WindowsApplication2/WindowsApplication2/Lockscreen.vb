'This is the form in which the user logs in, i.e puts his password to unlock. If he hasn't set a password, he is redirected to setPassword

Imports System.IO
Imports System
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text


Public Class Lockscreen
    Dim passwordHash As String
    Dim FILE_NAME As String = "output/pass.p" 'the path of password file

    'if set password has been clicked
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim setpass As SetPassword = New SetPassword
        setpass.Show()
        Me.Hide()

    End Sub

    Public Sub Lockscreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If System.IO.File.Exists(FILE_NAME) Then
            Using reader As StreamReader = New StreamReader(FILE_NAME, False)

                'if password file has been found, that means a password has been set
                'then set visible the unlock button and the text field
                Label1.Visible = True
                TextBox1.Visible = True

                passwordHash = reader.ReadLine()
                Button1.Visible = True

            End Using
        Else
            If (Not System.IO.Directory.Exists("output")) Then   'if output folder doesnt exist create it
                System.IO.Directory.CreateDirectory("output")
            End If
            'if no file found, we need to set password, so make those buttons visible
            Button2.Visible = True
            Label2.Visible = True

            'if no password file has been found or intentionally deleted, then delete all the previous entries(encrypted files)
            Dim di As New IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory() + "output")

            Dim allentries As IO.FileInfo() = di.GetFiles("*.encrypted")
            For Each dra In allentries
                My.Computer.FileSystem.DeleteFile("output/" + dra.Name)
            Next
        End If

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click 'handles unlock button
        'the next lines hash the given input similar to the way the password has been hashed. please look at SetPassword.vb
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

        'if this hash value matches the passwordhash stored in file, then enter program
        If hashValue = passwordHash Then
            Main.Show()
            Me.Hide()
        Else
            MessageBox.Show("Please enter the correct password")
        End If
    End Sub

End Class
