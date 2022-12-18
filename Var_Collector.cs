using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class Var_Collector : MonoBehaviour
{
    public class Var_Types
    {
        public enum Type
        {
            _int,
            _float,
            _string,
            _double,
        }
        public Type type;
        public int i_var;
        public float f_var;
        public string s_var;
        public double d_var;

        public Var_Types(int value)
        {
            i_var = value;
            type = Type._int;
        }
        public Var_Types(float value)
        {
            f_var = value;
            type = Type._float;
        }
        public Var_Types(string value)
        {
            s_var = value;
            type = Type._string;
        }
        public Var_Types(double value)
        {
            d_var = value;
            type = Type._double;
        }

        [JsonConstructor]
        public Var_Types(Type type, int i_var, float f_var, string s_var, double d_var)
        {
            this.type = type;
            this.i_var = i_var;
            this.f_var = f_var;
            this.s_var = s_var;
            this.d_var = d_var;
        }
    }

    private Dictionary<string, Var_Types> local_data;
    private Dictionary<string, Var_Types> server_data;

    public Var_Collector()
    {
        local_data = new Dictionary<string, Var_Types>();
    }

    public void set_data(string key, int value)
    {
        Var_Types tmp_var = new Var_Types(value);
        local_data.Add(key, tmp_var);
    }
    public void set_data(string key, float value)
    {
        Var_Types tmp_var = new Var_Types(value);
        local_data.Add(key, tmp_var);
    }
    public void set_data(string key, string value)
    {
        Var_Types tmp_var = new Var_Types(value);
        local_data.Add(key, tmp_var);
    }
    public void set_data(string key, double value)
    {
        Var_Types tmp_var = new Var_Types(value);
        local_data.Add(key, tmp_var);
    }

    public object get_data(string key)
    {
        switch (server_data[key].type)
        {
            case Var_Types.Type._int:
                return server_data[key].i_var;
            case Var_Types.Type._float:
                return server_data[key].f_var;
            case Var_Types.Type._string:
                return server_data[key].s_var;
            case Var_Types.Type._double:
                return server_data[key].d_var;
            default:
                return null;
        }
    }
 

    public void Serialize_local_data()
    {
        string jsonString = JsonConvert.SerializeObject(local_data);
        System.IO.StreamWriter writer = new System.IO.StreamWriter("ld.json", false);
        writer.WriteLine(jsonString);
        writer.Close();
    }
    public void Deserialize_server_data()
    {
        System.IO.StreamReader reader = new System.IO.StreamReader("sd.json");
        string jsonString = reader.ReadToEnd();
        server_data = JsonConvert.DeserializeObject<Dictionary<string, Var_Types>>(jsonString);
    }
    private void Update()
    {
        Deserialize_server_data();
        Serialize_local_data();
    }
}
