using IMGRED;

public static class Filters
{
  // -----------------------------
  //  Задание 1 — Негатив
  // -----------------------------
  public static void Invert(Image image)
  {
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);

        p.R = (byte)(255 - p.R);
        p.G = (byte)(255 - p.G);
        p.B = (byte)(255 - p.B);
      }
    }
  }

  // -----------------------------
  //  Задание 2 — Оттенки серого
  // -----------------------------
  public static void Grayscale(Image image)
  {
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);
        double intensity = 0.3 * p.R + 0.59 * p.G + 0.11 * p.B;

        p.R = p.G = p.B = (byte)intensity;
      }
    }
  }

  // -----------------------------
  //  Задание 3 — Сепия
  // -----------------------------
  public static void Sepia(Image image)
  {
    const int T = 20;
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);
        double intensity = 0.3 * p.R + 0.59 * p.G + 0.11 * p.B;

        p.R = Clamp((int)(intensity + 2 * T));
        p.G = Clamp((int)(intensity + T));
        p.B = Clamp((int)(intensity));
      }
    }
  }

  // -----------------------------
  //  Задание 4 — Яркость
  // -----------------------------

  public static void AdjustBrightness(Image image, sbyte value)
  {
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);
        AdjustPixelBrightness(ref p, value);
      }
    }
  }

  private static void AdjustPixelBrightness(ref Pixel p, sbyte value)
  {
    p.R = Clamp(p.R + value);
    p.G = Clamp(p.G + value);
    p.B = Clamp(p.B + value);
  }

  // -----------------------------
  //  Задание 5 — Контраст
  // -----------------------------
  public static void MakeContrast(Image image)
  {
    double factor = 1.5;
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);

        p.R = Clamp(128 + (int)((p.R - 128) * factor));
        p.G = Clamp(128 + (int)((p.G - 128) * factor));
        p.B = Clamp(128 + (int)((p.B - 128) * factor));
      }
    }
  }

  // -----------------------------
  //  Задание 6 — Box Blur
  // -----------------------------
  public static void BoxBlur(Image image)
  {
    image.Save("copy.bmp");
    Image copy = new Image("copy.bmp");

    for (int y = 1; y < image.Height-1; y++)
    {
      for (int x = 1; x < image.Width-1; x++)
      {
        int sumR = 0, sumG = 0, sumB = 0;

        for (int dy = -1; dy <= 1; dy++)
        {
          for (int dx = -1; dx <= 1; dx++)
          {
            Pixel p = copy[x + dx, y + dy];
            sumR += p.R;
            sumG += p.G;
            sumB += p.B;
          }
        }

        ref Pixel target = ref image.GetPixel(x, y);

        target.R = (byte)(sumR / 9);
        target.G = (byte)(sumG / 9);
        target.B = (byte)(sumB / 9);
      }
    }

    File.Delete("copy.bmp");
  }

  // -----------------------------
  //  Задание 7 — Изменение каналов
  // -----------------------------
  public static void AdjustChannels(Image image, sbyte dR, sbyte dG, sbyte dB)
  {
    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        ref Pixel p = ref image.GetPixel(x, y);

        p.R = Clamp(p.R + dR);
        p.G = Clamp(p.G + dG);
        p.B = Clamp(p.B + dB);
      }
    }
  }

  private static byte Clamp(int value)
  {
    if (value < 0) return 0;
    if (value > 255) return 255;
    return (byte)value;
  }
}
