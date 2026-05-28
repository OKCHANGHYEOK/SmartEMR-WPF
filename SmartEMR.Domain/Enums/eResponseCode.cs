namespace SmartEMR.Domain.Enums;

public enum eResponseCode
{
    SUCCESS = 200,
    CREATE_SUCCESS = 201,

    UNAUTHORIZED = 4001,     
    TOKEN_EXPIRED = 4002,    
    INVALID_TOKEN = 4003,  
    PERMISSION_DENIED = 4004,
    
    INVALID_PARAM = 5001, 
    DATA_NOTFOUND = 5002,   
    DUPLICATE_DATA = 5003,  
    
    INTERNAL_SERVER_ERROR = 9999
}
