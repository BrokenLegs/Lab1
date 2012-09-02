using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    class Logger{
        
        private List<string> _Logs = new List<string>();
             
        public void Log (string msg){
                        _Logs.Add (msg);
 
                        if (_Logs.Count >= 9){
                                _Logs.RemoveAt (0);
                        }
        }

        public override string ToString()
        {
            return String.Join("\n", _Logs);
        }


    }
}
