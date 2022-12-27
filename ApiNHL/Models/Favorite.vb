Imports MySql.Data.MySqlClient
Imports System.Net.WebUtility

Namespace Model
    Public Class Favorite
        Private _Name As String
        Private _UserId As Integer

        Public Sub New(Name As String, UserId As Integer)
            Me.Name = Name
            Me.UserId = UserId
        End Sub

        Public Sub New(Row As DataRow)
            Me.Name = HtmlEncode(Row.Item("name"))
            Me.UserId = Row.Item("user_id")
        End Sub

        Public Sub Save()
            DatabaseConnection.Save(Me)
        End Sub

        Public Sub Delete()

        End Sub

        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property

        Public Property UserId As Integer
            Get
                Return _UserId
            End Get
            Set(value As Integer)
                _UserId = value
            End Set
        End Property

        Public Shared Function GetAllFromLoggedInUser() As List(Of Favorite)
            Dim Cmd As New MySqlCommand With {
                .CommandText = "SELECT * FROM favorite_place WHERE user_id = @UserId"
            }

            Cmd.Parameters.AddWithValue("@UserId", ClaimManager.GetClaimValue("id"))

            Dim Table As DataTable = DatabaseConnection.GetDataTable(Cmd)

            Return Table.AsEnumerable.Select(Function(Row) New Favorite(Row)).ToList
        End Function
    End Class
End Namespace