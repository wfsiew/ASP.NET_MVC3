using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using mvcweb.App;

namespace mvcweb.Models
{
    public class Car
    {
        public virtual int ID { get; set; }

        [Required(ErrorMessage="Make is required.")]
        public virtual string Make { get; set; }

        [Required(ErrorMessage="Model is required.")]
        public virtual string Model { get; set; }

        [Required(ErrorMessage="Year is required.")]
        [Int(ErrorMessage="Year is invalid.")]
        public virtual int Year { get; set; }

        [Required(ErrorMessage="Doors is required.")]
        [Int(ErrorMessage ="Doors is invalid.")]
        public virtual int Doors { get; set; }

        [Required(ErrorMessage="Colour is required.")]
        public virtual string Colour { get; set; }

        [Required(ErrorMessage="Price is required.")]
        [Double(ErrorMessage="Price is invalid.")]
        public virtual double Price { get; set; }

        internal static Dictionary<string, object> GetErrors(ModelStateDictionary ms)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, string> m = new Dictionary<string, string>();

            foreach (KeyValuePair<string, ModelState> o in ms)
            {
                string f = o.Key;

                if (o.Value.Errors.Count > 0)
                {
                    string s = o.Value.Errors[0].ErrorMessage;
                    m.Add(f, s);
                }
            }

            dic.Add("error", 1);
            dic.Add("errors", m);

            return dic;
        }
    }
}