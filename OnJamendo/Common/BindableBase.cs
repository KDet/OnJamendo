﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OnJamendo.Common
{
    /// <summary>
    /// Реализация интерфейса <see cref="INotifyPropertyChanged"/> для упрощения моделей.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Многоадресное событие для уведомлений об изменении свойств.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Проверяет, не равно ли уже свойство требуемому значению. Задает свойство и
        /// уведомляет прослушиватели только при необходимости.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="storage">Ссылка на свойство, имеющее методы задания и получения.</param>
        /// <param name="value">Требуемое значение свойства.</param>
        /// <param name="propertyName">Имя свойства, используемого для уведомления прослушивателей. Это
        /// значение является необязательным, оно может предоставляться автоматически при вызове из компилятора, который
        /// поддерживает атрибут CallerMemberName.</param>
        /// <returns>Значение true при изменении значения, значение false, если существующее значение совпадает с
        /// требуемым значением.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Уведомляет прослушиватели об изменении значения свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства, используемого для уведомления прослушивателей. Это
        /// значение является необязательным, оно может предоставляться автоматически при вызове из компилятора,
        /// который поддерживает атрибут <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
