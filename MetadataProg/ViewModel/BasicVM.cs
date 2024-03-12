using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MetadataProg.ViewModel
{
    /// <summary>
    /// Класс для обработки DataBinding MVVM.
    /// </summary>
    public class BasicVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Обновление параметра при изменении.
        /// </summary>
        /// <param name="PropertyName"> Название свойства. </param>
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Обновление элемента.
        /// </summary>
        /// <typeparam name="T"> Любой тип. </typeparam>
        /// <param name="field"> Поле. </param>
        /// <param name="value"> Значение. </param>
        /// <param name="PropertyName"> Название свойства. </param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName); return true;
        }
    }
}
