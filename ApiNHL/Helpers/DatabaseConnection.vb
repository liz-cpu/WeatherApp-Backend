Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Public Class DatabaseConnection
    Private Shared Function GetConnection() As MySqlConnection
        Return New MySqlConnection(DataProtector.Unprotect(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ToString))
    End Function

    ''' <summary>
    ''' Voert de meegegeven Sql Command uit en vult een DataTable met het resultaat
    ''' van de query.
    ''' </summary>
    ''' <param name="Cmd">Een Sql Command met een query.</param>
    ''' <returns>Een Datatable die gevuld is met de data van het query resultaat.</returns>
    Public Shared Function GetDataTable(Cmd As MySqlCommand) As DataTable
        Dim Conn As MySqlConnection = GetConnection()
        Conn.Open()
        Cmd.Connection = Conn

        Dim Table As New DataTable

        Using Cmd
            Using Adapter As New MySqlDataAdapter(Cmd)
                Adapter.Fill(Table)
            End Using
        End Using

        Conn.Close()

        Return Table
    End Function

    ''' <summary>
    ''' Voert de sql query uit die is meegegeven.
    ''' </summary>
    ''' <param name="Cmd">Een Sql Command met een query.</param>
    ''' <returns>De waarde uit de eerste rij uit de eerste kolom van het resultaat.</returns>
    Public Shared Function ExecuteScalar(Cmd As MySqlCommand) As Object
        Dim Conn As MySqlConnection = GetConnection()
        Conn.Open()
        Cmd.Connection = Conn

        Dim Value As Object = Cmd.ExecuteScalar

        Conn.Close()
        Return Value
    End Function

    ''' <summary>
    ''' Voert de sql query uit die is meegegeven.
    ''' </summary>
    ''' <param name="Cmd">Een Sql Command met een query.</param>
    ''' <returns>De hoeveelheid regels die zijn aangepast met de query.</returns>
    Public Shared Function ExecuteQuery(Cmd As MySqlCommand) As Integer
        Dim Conn As MySqlConnection = GetConnection()
        Conn.Open()
        Cmd.Connection = Conn

        Dim Rows As Integer = Cmd.ExecuteNonQuery()

        Conn.Close()

        Return Rows
    End Function

    Public Shared Function GetFirstRow(Cmd As MySqlCommand) As DataRow
        Dim Table As DataTable = GetDataTable(Cmd)

        If Table.Rows.Count.Equals(0) Then
            Return Nothing
        End If
        Return Table.Rows.Item(0)
    End Function

    Public Shared Sub Open(Obj As Model.User)
        Dim Cmd As New MySqlCommand

        If Obj.Id > 0 Then
            Cmd.CommandText = "SELECT * FROM user WHERE id = @Id LIMIT 1"
            Cmd.Parameters.AddWithValue("@Id", Obj.Id)
        ElseIf Not IsNothing(Obj.Username) Then
            Cmd.CommandText = "SELECT * FROM user WHERE username = @Username LIMIT 1"
            Cmd.Parameters.AddWithValue("@Username", Obj.Username)
        Else
            Exit Sub
        End If

        Dim Row As DataRow = GetFirstRow(Cmd)

        If Not IsNothing(Row) Then
            Obj.Fill(Row)
            Exit Sub
        End If
        Obj.Empty()
    End Sub

    Public Shared Function Save(Obj As Model.User) As Integer
        Dim Cmd As New MySqlCommand

        If Obj.Id.Equals(0) Then
            Cmd.CommandText = "INSERT INTO user (username, password) VALUES (@Username, @Password);SELECT LAST_INSERT_ID()"
            Cmd.Parameters.AddWithValue("@Username", Obj.Username)
        Else
            Cmd.CommandText = "UPDATE tbl user SET password = @Password WHERE id = @Id"
            Cmd.Parameters.AddWithValue("@Id", Obj.Id)
        End If

        Cmd.Parameters.AddWithValue("@Password", Obj.Password)

        If Obj.Id.Equals(0) Then
            Return ExecuteScalar(Cmd)
        End If

        ExecuteQuery(Cmd)
        Return Obj.Id
    End Function

    Public Shared Sub Save(Obj As Model.Favorite)
        Dim Cmd As New MySqlCommand With {
            .CommandText = "INSERT INTO favorite_place (name, user_id) VALUES (@Name, @UserId)"
        }

        Cmd.Parameters.AddWithValue("@Name", Obj.Name)
        Cmd.Parameters.AddWithValue("@UserId", Obj.UserId)

        ExecuteQuery(Cmd)
    End Sub

    Public Shared Sub Delete(Obj As Model.Favorite)
        Dim Cmd As New MySqlCommand With {
            .CommandText = "DELETE FROM favorite_place WHERE name = @Name AND user_id = @UserId"
        }

        Cmd.Parameters.AddWithValue("@Name", Obj.Name)
        Cmd.Parameters.AddWithValue("@UserId", Obj.UserId)

        ExecuteQuery(Cmd)
    End Sub
End Class
