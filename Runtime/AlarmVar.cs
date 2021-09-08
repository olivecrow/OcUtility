using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace OcUtility
{
    public struct AlarmVar<T>
    {
        public T value
        {
            get => _value;
            set
            {
                var isNew = !value.Equals(_value);
                var from = _value;
                _value = value;
                if (isNew) OnValueChanged?.Invoke(from, value);
            }
        }

        T _value;

        public event Action<T, T> OnValueChanged;

        public AlarmVar(T t)
        {
            _value = t;
            OnValueChanged = null;
        }

        public void SetValueWithoutNotify(T t)
        {
            _value = t;
        }

        public void ReleaseEvents()
        {
            OnValueChanged = null;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public bool Equals(AlarmVar<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
        
        public static bool operator ==(AlarmVar<T> t1, T t2)
        {
            return t1._value.Equals(t2);
        }

        public static bool operator !=(AlarmVar<T> t1, T t2)
        {
            return !t1._value.Equals(t2);
        }
    }
}