namespace StandAloneExercises.Pricing
{
    public class PriceTick
    {
        public bool IsValid { get; set; }
        public decimal Price { get; set; }
        public decimal Delta { get; set; }

        #region Equality operators

        protected bool Equals(PriceTick other)
        {
            return IsValid.Equals(other.IsValid) && Price == other.Price && Delta == other.Delta;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = IsValid.GetHashCode();
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ Delta.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PriceTick)obj);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Price:{0} Delta:{1}, IsValid:{2}", Price, Delta, IsValid);
        }
    }
}