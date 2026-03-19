namespace SampleOrg.Foo.Website.Models;

public static class ColorData
{
    public static readonly ColorEntry[] AllColors =
    [
        new( 1, "Red",             "red",             "Basic",        5,   "#CC0000"),
        new( 2, "Reddish Brown",   "reddish-brown",   "Intermediate", 12,  "#7B2D00"),
        new( 3, "Reddish Orange",  "reddish-orange",  "Intermediate", 17,  "#E04000"),
        new( 4, "Brown",           "brown",           "Basic",        24,  "#8B4513"),
        new( 5, "Orange",          "orange",          "Basic",        30,  "#E07800"),
        new( 6, "Brownish Orange", "brownish-orange", "Intermediate", 36,  "#B35A00"),
        new( 7, "Yellowish Brown", "yellowish-brown", "Intermediate", 43,  "#9B6B00"),
        new( 8, "Orange Yellow",   "orange-yellow",   "Basic",        46,  "#E8A800"),
        new( 9, "Olive Brown",     "olive-brown",     "Intermediate", 52,  "#8B7000"),
        new(10, "Yellow",          "yellow",          "Basic",        60,  "#F0D000"),
        new(11, "Olive",           "olive",           "Basic",        66,  "#6B7000"),
        new(12, "Greenish Yellow", "greenish-yellow", "Intermediate", 73,  "#C8D400"),
        new(13, "Yellow Green",    "yellow-green",    "Basic",        80,  "#88B800"),
        new(14, "Olive Green",     "olive-green",     "Intermediate", 87,  "#5A7A00"),
        new(15, "Yellowish Green", "yellowish-green", "Intermediate", 97,  "#44A000"),
        new(16, "Green",           "green",           "Basic",        120, "#009900"),
        new(17, "Bluish Green",    "bluish-green",    "Intermediate", 150, "#007755"),
        new(18, "Blue Green",      "blue-green",      "Basic",        165, "#006688"),
        new(19, "Greenish Blue",   "greenish-blue",   "Intermediate", 194, "#0077AA"),
        new(20, "Blue",            "blue",            "Basic",        220, "#0044BB"),
        new(21, "Purplish Blue",   "purplish-blue",   "Intermediate", 234, "#3333BB"),
        new(22, "Bluish Violet",   "bluish-violet",   "Intermediate", 245, "#5522CC"),
        new(23, "Violet",          "violet",          "Basic",        265, "#7700CC"),
        new(24, "Bluish Purple",   "bluish-purple",   "Intermediate", 275, "#6600CC"),
        new(25, "Purple",          "purple",          "Basic",        290, "#9900BB"),
        new(26, "Reddish Purple",  "reddish-purple",  "Intermediate", 310, "#AA0077"),
        new(27, "Purplish Red",    "purplish-red",    "Intermediate", 325, "#BB0055"),
        new(28, "Purplish Pink",   "purplish-pink",   "Intermediate", 338, "#FF77CC"),
        new(29, "Pink",            "pink",            "Basic",        350, "#FFB3BA"),
        // Achromatic neutrals (no hue — sorted last)
        new(30, "White",           "white",           "Neutral",      999, "#FFFFFF"),
        new(31, "Gray",            "gray",            "Neutral",      999, "#808080"),
        new(32, "Black",           "black",           "Neutral",      999, "#222222"),
    ];

    public static readonly IReadOnlyDictionary<string, ColorEntry> BySlug =
        AllColors.ToDictionary(c => c.Slug);
}
