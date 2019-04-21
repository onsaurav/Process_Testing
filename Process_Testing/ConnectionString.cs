/****************************************
 * Project: GCDAO.DBConnectionUtility
 * Class:   configuration
 * Author:  ah
 * Version: 1.0 
 * Created: 01/28/2007 10:33:24
 * 
 * 
 * Modified Author:
 * Date:
 * Fields of Modification:
 * 
 * 
 * Copyright Year: 2007  
 ****************************************/



public class configuration {

    private string connectionStringsField;

    public string connectionStrings 
    {
        get {
            return this.connectionStringsField;
        }
        set {
            this.connectionStringsField = value;
        }
    }
}


