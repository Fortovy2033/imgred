using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGRED
{
  public class Image
  {
    private Pixel[,] _pixels;

    public int Width
    {
      get
      { return _pixels.GetLength(0); }
    }

    public int Height
    {
      get
      { return _pixels.GetLength(1); }
    }

    public Image(string filePath)
    {
      using (var reader = new BinaryReader(File.OpenRead(filePath)))
      {
        if (reader.ReadChar() != 'B' || reader.ReadChar() != 'M')
            throw new ArgumentException("Неверный формат файла. Ожидался BMP.");

        reader.ReadInt32(); // Размер файла (не используем)
        reader.ReadInt16(); // Зарезервировано
        reader.ReadInt16(); // Зарезервировано
        int dataOffset = reader.ReadInt32(); // Смещение до начала данных пикселей

        // --- Чтение информационного заголовка ---
        reader.ReadInt32(); // Размер инфо-заголовка (не используем)
        int width = reader.ReadInt32();
        int height = reader.ReadInt32();
        reader.ReadInt16(); // Цветовые плоскости (должно быть 1)
        short bitsPerPixel = reader.ReadInt16();

        if (bitsPerPixel != 24)
          throw new NotSupportedException("Поддерживаются только 24-битные BMP изображения.");

        _pixels = new Pixel[width, height];

        int padding = (4 - (width * 3) % 4) % 4;

        reader.BaseStream.Seek(dataOffset, SeekOrigin.Begin);

        for (int y = height - 1; y >= 0; y--)
        {
          for (int x = 0; x < width; x++)
          {
            byte b = reader.ReadByte();
            byte g = reader.ReadByte();
            byte r = reader.ReadByte();
            _pixels[x, y] = new Pixel { R = r, G = g, B = b };
          }
          reader.ReadBytes(padding);
        }
      }
    }

    public void Save(string filePath)
    {
      int width = Width;
      int height = Height;
      int padding = (4 - (width * 3) % 4) % 4;
      int imageSize = (width * 3 + padding) * height;
      int fileSize = 54 + imageSize;

      using (var writer = new BinaryWriter(File.Create(filePath)))
      {
        // --- Запись заголовка BMP (14 байт) ---
        writer.Write('B');
        writer.Write('M');
        writer.Write(fileSize);
        writer.Write((short)0);
        writer.Write((short)0);
        writer.Write(54); // Смещение до данных

        // --- Запись информационного заголовка (40 байт) ---
        writer.Write(40); // Размер инфо-заголовка
        writer.Write(width);
        writer.Write(height);
        writer.Write((short)1); // Плоскости
        writer.Write((short)24); // Бит на пиксель
        writer.Write(0); // Без сжатия
        writer.Write(imageSize);
        writer.Write(0); // Разрешение по X (неважно)
        writer.Write(0); // Разрешение по Y (неважно)
        writer.Write(0); // Количество цветов в палитре
        writer.Write(0); // Количество важных цветов

        for (int y = height - 1; y >= 0; y--)
        {
          for (int x = 0; x < width; x++)
          {
            Pixel p = _pixels[x, y];
            writer.Write(p.B);
            writer.Write(p.G);
            writer.Write(p.R);
          }

          for (int i = 0; i < padding; i++)
            writer.Write((byte)0);
        }
      }
    }

    public Pixel this[int x, int y]
    {
      get
      { return _pixels[x, y]; }
      set
      { _pixels[x, y] = value; }
    }

    public ref Pixel GetPixel(int x, int y)
    {
      if (x < 0 || x >= Width || y < 0 || y >= Height)
        throw new IndexOutOfRangeException($"Координаты ({x},{y}) выходят за пределы изображения.");

      return ref _pixels[x, y];
    }
  }
}