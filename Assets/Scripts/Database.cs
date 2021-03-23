using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class Database : MonoBehaviour
{
    public string MONGO_URI;
    public string DATABASE_NAME;
    public string PLAYERDATA_COLLECTION_NAME;
    public string MATCHDATA_COLLECTION_NAME;
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> playerData;
    private IMongoCollection<BsonDocument> matchData;

    public void Init(Character left, Character right)
    {
        left.ToBson();
    }

    void Start()
    {
        client = new MongoClient(MONGO_URI);
        db = client.GetDatabase(DATABASE_NAME);
        playerData = db.GetCollection<BsonDocument>(PLAYERDATA_COLLECTION_NAME);
        matchData = db.GetCollection<BsonDocument>(MATCHDATA_COLLECTION_NAME);
    }

    void Update()
    {
        
    }

    public string LoadPlayer(string PlayerID)
    {
        
        return "playerData";
    }
    public void SavePlayer(string PlayerJson)
    {

    }
}
