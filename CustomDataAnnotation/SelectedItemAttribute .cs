using System.ComponentModel.DataAnnotations;

namespace WebApp.CustomDataAnnotation
{
    public class SelectedItemDataAttribute : ValidationAttribute
    {
        public SelectedItemDataAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if ((int)value <= 1)
            {
                return false;
            }
            return true;
        }
    }
}
