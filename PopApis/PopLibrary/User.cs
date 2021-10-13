namespace PopLibrary
{
    /// <summary>
    /// Represents an authenticated user.
    /// </summary>
    public record User
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string Role { get; init; }
    }
}
