using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data 
{
    public int Dia;
    public int Mes;
    public int Ano;

    public Data(int dia, int mes, int ano)
    {
        Dia = dia;
        Mes = mes;
        Ano = ano;
    }
    public Data(string str)
    {
        Dia = int.Parse(str.Substring(0,2));
        Mes = int.Parse(str.Substring(3,2));
        Ano = int.Parse(str.Substring(6,2));
    }

    //retorna o dia de hj
    public Data(System.DateTime dateTime)
    {
        Dia = dateTime.Day;
        Mes = dateTime.Month;
        Ano = dateTime.Year;
    }

    public override string ToString()
    {
        string d = Dia.ToString();
        string m = Mes.ToString();
        string a = Ano.ToString();
        if (Dia < 10)
            d = "0" + d;
        if (Mes < 10)
            m = "0" + m;

        return d + "/" + m + "/" + a;
    }


    //Comparisons

    public override bool Equals(object p)
    {
        // If parameter is null, return false.
        if (Object.ReferenceEquals(p, null))
        {
            return false;
        }

        // Optimization for a common success case.
        if (Object.ReferenceEquals(this, p))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != p.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        var second = (Data)p;
        return (Dia == second.Dia) && (Mes == second.Mes) && (Ano == second.Ano);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public int CompareTo(object p)
    {
        var second = (Data)p;
        if( second < this)
        {
            return -1;
        }
        if (second > this)
        {
            return 1;
        }
        return 0;
    }

    public static bool operator ==(Data lhs, Data rhs)
    {
        // Check for null on left side.
        if (Object.ReferenceEquals(lhs, null))
        {
            if (Object.ReferenceEquals(rhs, null))
            {
                // null == null = true.
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Data lhs, Data rhs)
    {
        return !(lhs == rhs);
    }
    public static bool operator <(Data lhs, Data rhs)
    {
        return DateTime(lhs) < DateTime(rhs);
    }
    public static bool operator >(Data lhs, Data rhs)
    {
        return DateTime(lhs) > DateTime(rhs);
    }
    public static bool operator <=(Data lhs, Data rhs)
    {
        return !( DateTime(lhs) > DateTime(rhs));
    }
    public static bool operator >=(Data lhs, Data rhs)
    {
        return !( DateTime(lhs) < DateTime(rhs));
    }

    


    //Static methods
    public static bool EstaEntre(Data data, Data minima, Data maxima)
    {
        //Debug.Log(data.ToString() + " está entre " + minima.ToString() + " e " + maxima.ToString() + "?\n" + (data >= minima && data <= maxima));
        return (data >= minima && data <= maxima) ;
    }

    public static System.DateTime DateTime(Data data)
    {
        return new System.DateTime(data.Ano, data.Mes, data.Dia);
    }

    public static bool IsValid(string data)
    {
        if( System.DateTime.TryParse(data, out System.DateTime temp))
        {
            return true;
        }

        return false;
    }

}
