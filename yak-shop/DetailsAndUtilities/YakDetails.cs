namespace yak_shop.DetailsAndUtilities
{
    public class YakDetails
    {
        public string Name;
        public float Age { get; set; }
        public char Sex { get; set; }
        public float ageLastShaved { get; set; }

        public YakDetails(string name, float age, char sex)
        {
            Name = name;
            Age = age;
            Sex = sex;
        }
    }
}
