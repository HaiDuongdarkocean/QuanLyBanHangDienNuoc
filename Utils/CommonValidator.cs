using System;

namespace Validators
{
    public static class CommonValidator
    {
        /// <summary>
        /// Kiểm tra số nguyên dương
        /// </summary>
        public static void KiemTraSoDuong(int giaTri, string tenTruong)
        {
            if (giaTri <= 0)
            {
                throw new ArgumentException($"{tenTruong} phải lớn hơn 0.");
            }
        }
        /// <summary>
        /// Kiểm tra số thực dương
        /// </summary>
        public static void KiemTraSoDuong(decimal giaTri, string tenTruong)
        {
            if (giaTri <= 0)
            {
                throw new ArgumentException($"{tenTruong} phải là số dương.");
            }
        }

        /// <summary>
        /// check if the string is empty
        /// </summary>
        public static void KiemTraChuoiRong(string giaTri, string tenTruong)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
            {
                throw new ArgumentException($"{tenTruong} không được để trống.");
            }
        }

        /// <summary>
        /// check if the date is valid
        /// </summary>
        public static void KiemTraNgay(DateTime ngay, string tenTruong)
        {
            if (ngay == DateTime.MinValue)
            {
                throw new ArgumentException($"{tenTruong} không hợp lệ.");
            }
        }
        /// <summary>
        /// Display error message
        /// </summary>
        public static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        /// <summary>
        /// Display success message
        /// </summary>
        public static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static bool ConfirmHuy()
        {
            Console.Write("❗ Bạn có chắc chắn muốn hủy thao tác này? (Y/N): ");
            string confirm = Console.ReadLine()?.Trim().ToUpper();
            return confirm == "Y";
        }

        public static bool ConfirmNhapLai(string mes = "Bạn có muốn nhập lại không?")
        {
            string confirm;
            do
            {
                Console.Write($"❗ {mes} (Y/N): ");
                confirm = Console.ReadLine()?.Trim().ToUpper();

                if (confirm == "Y") return true;
                if (confirm == "N") return false;

                Console.WriteLine("⚠️ Vui lòng nhập Y hoặc N!");
            }
            while (true);
        }

        public static void ConfirmAndRetry(Action callback, string message = "❗ Bạn có muốn tiếp tục thao tác này không? (Y/N): ")
        {
            Console.Write(message);
            string input = Console.ReadLine()?.Trim().ToUpper();
            if (input == "Y")
            {
                callback(); // Gọi lại chính phương thức đã gọi ConfirmAndRetry
            }
            else if (input == "N")
            {
                Console.WriteLine("❌ Đã hủy thao tác!");
                return;
            }
            else
            {
                Console.WriteLine("⚠️ Vui lòng nhập Y hoặc N!");
            }
        }

        public static string PromptString(string message)
        {
            Console.Write(message);
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n❌ Đã hủy thao tác!");
                    return null;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine();
                        return input;
                    }
                    else
                    {
                        Console.Write("\n❌ Không được để trống! Nhập lại: ");
                        input = "";
                    }
                }
                else
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
        }

        public static int PromptInt(string message)
        {
            Console.Write(message);
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n❌ Đã hủy thao tác!");
                    return -1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (int.TryParse(input, out int value) && value > 0)
                    {
                        Console.WriteLine();
                        return value;
                    }
                    else
                    {
                        Console.Write("\n❌ Giá trị không hợp lệ! Nhập lại: ");
                        input = "";
                    }
                }
                else if (char.IsDigit(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
        }

        public static decimal PromptDecimal(string message, bool allowEmpty = false)
        {
            Console.Write(message);
            string input = "";
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n❌ Đã hủy thao tác!");
                    return -1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (allowEmpty && string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine();
                        return -1;
                    }
                    if (decimal.TryParse(input, out decimal value) && value >= 0)
                    {
                        Console.WriteLine();
                        return value;
                    }
                    else
                    {
                        Console.Write("\n❌ Giá trị không hợp lệ! Nhập lại: ");
                        input = "";
                    }
                }
                else if (char.IsDigit(key.KeyChar) || key.KeyChar == '.')
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
        }

        public static bool ConfirmAction(string message, string no = "")
        {
            while (true)
            {
                Console.Write($"\n{message} (Y/N): ");
                string confirm = Console.ReadLine()?.Trim().ToUpper();

                if (confirm == "Y") return true;
                if (confirm == "N")
                {
                    if (!string.IsNullOrEmpty(no))
                        Console.WriteLine(no);
                    return false;
                }

                Console.WriteLine("⚠️ Vui lòng nhập Y hoặc N!");
            }
        }


    }
}
