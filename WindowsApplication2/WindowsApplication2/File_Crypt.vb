'This is a class with two functions encrypt and decrypt which are used again and again in the software to encrypt and decrypt files

Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Public Class File_Crypt
    Sub EncryptFile(ByVal sInputFilename As String, _
                   ByVal sOutputFilename As String, _
                   ByVal sKey As String)
        'input file stream
        Dim fsInput As New FileStream(sInputFilename, _
                                    FileMode.Open, FileAccess.Read)
        'encrypted file stream
        Dim fsEncrypted As New FileStream(sOutputFilename, _
                                    FileMode.Create, FileAccess.Write)

        Dim DES As New DESCryptoServiceProvider()
        'to enable us to use DES algorithm

        'converts the key given to bytes and encodes it into ascii
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey)

        'the initialization of the vector for DES
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)

        'DES encryption is created from this instance
        Dim desencrypt As ICryptoTransform = DES.CreateEncryptor()
        'this transforms the sentence into some sequence
        'using DES encryption
        Dim cryptostream As New CryptoStream(fsEncrypted, _
                                            desencrypt, _
                                            CryptoStreamMode.Write)

        'Read the text file into the byte array
        Dim bytearrayinput(fsInput.Length - 1) As Byte
        fsInput.Read(bytearrayinput, 0, bytearrayinput.Length)

        'Write the file encryption with DES
        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length)

        cryptostream.Close()
        fsEncrypted.Close()
        fsInput.Close()

        'delete the txt file which is given as input
        My.Computer.FileSystem.DeleteFile(sInputFilename)
    End Sub

    Sub DecryptFile(ByVal sInputFilename As String, _
        ByVal sOutputFilename As String, _
        ByVal sKey As String)

        Dim DES As New DESCryptoServiceProvider()
        'It requires a 64-bit key and IV for this provider.
        'Set the secret key for the DES algorithm

        DES.Key() = ASCIIEncoding.ASCII.GetBytes(sKey)
        'Set the initialization vector.
        DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey)

        'create the file stream back to read the encrypted file
        Dim fsread As New FileStream(sInputFilename, FileMode.Open, FileAccess.Read)
        'create descriptor DES from our instance of DES
        Dim desdecrypt As ICryptoTransform = DES.CreateDecryptor()
        'create sequences set to read and perform encryption
        'DES decryption transformation on the incoming bytes
        Dim cryptostreamDecr As New CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read)
        'print the decrypted file content
        Dim fsDecrypted As New StreamWriter(sOutputFilename)
        fsDecrypted.Write(New StreamReader(cryptostreamDecr).ReadToEnd)
        fsDecrypted.Flush() 'clears all buffers
        fsDecrypted.Close()
        cryptostreamDecr.Close()
        fsread.Close()
    End Sub

End Class