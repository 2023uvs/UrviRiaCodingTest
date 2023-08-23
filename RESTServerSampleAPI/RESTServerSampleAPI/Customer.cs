namespace RESTServerSampleAPI
{
    public class Customer
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public int Age { get; set; }
        public int Id { get; set; }
    }

    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer? x, Customer? y)
        {
            int lastNameComparison = x.LastName.CompareTo(y.LastName);
            if (lastNameComparison == 0)
                return x.FirstName.CompareTo(y.FirstName);
            return lastNameComparison;
        }
    }
}