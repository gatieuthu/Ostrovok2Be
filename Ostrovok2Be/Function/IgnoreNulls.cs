using System.Reflection;
using Omu.ValueInjecter.Injections;

namespace Ostrovok2Be.Function
{
    public class IgnoreNulls : LoopInjection
    {
        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {


            var val = sp.GetValue(source);
            var val_tar = tp.GetValue(target);
            if (val != null)
            {
                
                    if (val_tar != null || val.ToString() != "")
                {
                    tp.SetValue(target, val);
                }
              /*  else
                    tp.SetValue(target, val);*/
            }
        }
    }
}