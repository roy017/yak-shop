namespace yak_shop.DetailsAndUtilities
{
    public class YakDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Age { get; set; }
        public char Sex { get; set; }
        public float ageLastShaved { get; set; }

        public YakDetails()
        {

        }
        public YakDetails(string name, float age, char sex, float lastShaved)
        {
            Name = name;
            Age = age;
            Sex = sex;
            ageLastShaved = lastShaved;
        }
    }
}
