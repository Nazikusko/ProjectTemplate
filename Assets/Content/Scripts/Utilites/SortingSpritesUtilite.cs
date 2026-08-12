public static class SortingSpritesUtilite
{
    public static int GetSortingOrderByZPosition(float zPosition)
    {
        return (-(int)(zPosition * 5)) + 30000;
    }
}
