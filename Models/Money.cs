using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCoonConsole.Models
{
    public class Money
    {

        private decimal value;

        public decimal Value
        {
            set { this.value = value; }
            get { return Decimal.Round(value, 2); }
        }

  public decimal PrecisionValue
        {
            get { return this.value; }
        }

        // Constructor with value
        public Money(Decimal value)
        {
            this.value = value;
        } // End Constructor

        // Constructor with value
        public Money(int value)
        {
            this.value = (Decimal)value;
        } // End Constructor
        public Money(Int64 value)
        {
            this.value = (Decimal)value;
        } // End Constructor


        public static Money operator +(Money m1, Money m2)
        {
            return new Money(m1.PrecisionValue + m2.PrecisionValue);
        }
        public static Money operator -(Money m1, Money m2)
        {
            return new Money(m1.PrecisionValue - m2.PrecisionValue);
        }

        public static bool operator ==(Money m1, Money m2)
        {
            return m1.Value == m2.Value;
        }
        public static bool operator !=(Money m1, Money m2)
        {
            return m1.Value != m2.Value;
        }
        public static bool operator >(Money m1, Money m2)
        {
            return m1.Value > m2.Value;
        }
        public static bool operator <(Money m1, Money m2)
        {
            return m1.Value < m2.Value;
        }
        public static bool operator <=(Money m1, Money m2)
        {
            return m1.Value <= m2.Value;
        }
        public static bool operator >=(Money m1, Money m2)
        {
            return m1.Value >= m2.Value;
        }

        public override bool Equals(object obj)
        {
            var money = obj as Money;
            return money != null &&
                   PrecisionValue == money.PrecisionValue;
        }

        public override int GetHashCode()
        {
            return 1363966828 + PrecisionValue.GetHashCode();
        }
    }
}
