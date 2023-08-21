using TMPro;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;
using UnityEditor;
using System;
using UnityEngine.UI;

public class SaveScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI applesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TMP_InputField inputName;
    private int apples;
    private int time;

    private string connectionString;

    private void Start()
    {
        apples = ItemCollector.apples;
        time = (int)ItemCollector.timer;

        applesText.text = "Apples: " + apples;
        timerText.text = "Time: " + time;

        Debug.Log("Connecting to database...");

        connectionString = "Server=tcp:shared.database.windows.net,1433;Initial Catalog=scaleGuy;Persist Security Info=False;User ID=user;Password=9o48LPobi0tmFblCOftc;" +
            "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        using (SqlConnection dbConnection = new SqlConnection(connectionString))
        try
        {
            dbConnection.Open();
            Debug.Log("Connected to database.");
        }
        catch (SqlException exception)
        {
            Debug.LogWarning(exception.ToString());
        }
    }

    public void WriteScore()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Connected to databse.");

                Guid guid = Guid.NewGuid();
                DateTime now = DateTime.Now;
                var username = inputName.text.ToString();

                using (SqlCommand command = CreateCommand(connection, "INSERT INTO scoreboard ([Id], [Name], [Score], [Time], [CreatedAt]) VALUES" +
                                                                                                "(@id, @name, @score, @time, @createdAt)"))
                {
                    AddParamater(command, "@id", SqlDbType.UniqueIdentifier, guid);
                    AddParamater(command, "@name", SqlDbType.NVarChar, username);
                    AddParamater(command, "@score", SqlDbType.Int, apples);
                    AddParamater(command, "@time", SqlDbType.Int, time);
                    AddParamater(command, "@createdAt", SqlDbType.DateTime, now);

                    command.ExecuteNonQuery();
                }

                Debug.Log(inputName.text);

                Debug.Log("Score saved to the database successfully.");
            }
        }
        catch (SqlException exception)
        {
            Debug.LogWarning(exception.ToString());
        }
    }

    private SqlCommand CreateCommand(SqlConnection connection, string sql)
    {
        SqlCommand command = connection.CreateCommand();
        command.CommandText = sql;

        return command;
    }

    private void AddParamater(SqlCommand command, string parameterName, SqlDbType parameterType, object paramaterValue)
    {
        command.Parameters.Add(parameterName, parameterType).Value = paramaterValue;
    }

    public void ShowScore()
    {
        Debug.Log(apples);
        Debug.Log(time);
    }
}
