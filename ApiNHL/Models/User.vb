Imports System.Data

Namespace Model
    Public Class User
        Private _Id As Integer
        Private _Username As String
        Private _Password As String

        Public Sub New(Id As Integer)
            Me.Id = Id
            DatabaseConnection.Open(Me)
        End Sub

        Public Sub New(Username As String)
            Me.Username = Username
            DatabaseConnection.Open(Me)
        End Sub

        Public Sub New(Username As String, Password As String)
            Me.Username = Username
            Me.Password = Password
        End Sub

        ''' <summary>
        ''' Slaat de gebruiker op in de database. Als het een nieuwe gebruiker is wordt er 
        ''' gecheckt of de gebruikersnaam nog vrij is.
        ''' </summary>
        ''' <returns>True als er een database wijziging was, anders False.</returns>
        Public Function Save() As Boolean
            If Me.Id.Equals(0) AndAlso Me.UsernameExists Then
                Return False
            End If
            Me.Id = DatabaseConnection.Save(Me)
            Return True
        End Function

        Friend Sub Fill(Row As DataRow)
            Me.Id = Row.Item("id")
            Me.Username = Row.Item("username")
            Me.Password = Row.Item("password")
        End Sub

        Friend Sub Empty()
            Me.Id = 0
            Me.Username = Nothing
            Me.Password = Nothing
        End Sub

        Public Property Id As Integer
            Get
                Return _Id
            End Get
            Private Set(value As Integer)
                _Id = value
            End Set
        End Property

        Public Property Username As String
            Get
                Return _Username
            End Get
            Private Set(value As String)
                _Username = value
            End Set
        End Property

        Public Property Password As String
            Get
                Return _Password
            End Get
            Private Set(value As String)
                _Password = value
            End Set
        End Property

        Private Function UsernameExists() As Boolean
            Dim Usr As New User(Me.Username)
            Return Usr.Id > 0
        End Function
        ''' <summary>
        ''' Kijkt of de gebruikersnaam en wachtwoord een match zijn.
        ''' </summary>
        ''' <param name="Username">Gebruikersnaam</param>
        ''' <param name="Password">Wachtwoord</param>
        ''' <returns>Een instantie van de user object als gebruikersnaam en wachtwoord overeen komen, anders Nothing</returns>
        Public Shared Function Login(Username As String, Password As String) As User
            Dim Usr As New User(Username)

            If Usr.Id.Equals(0) Then
                Return Nothing
            End If

            If Cryptographic.ValidatePassword(Password, Usr.Password) Then
                Return Usr
            End If
            Return Nothing
        End Function
    End Class
End Namespace