using Npgsql;
using System;
using System.Linq;
using Xamarin.Forms;

namespace DarkFit_app
{
    public partial class SettingsPage : ContentPage
    {
        private int _roleId;
        private bool _isFormattingPhone;

        public SettingsPage()
        {
            InitializeComponent();
            LoadUserInfo();
        }

        private async void LoadUserInfo()
        {
            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    // Получение логина и роли
                    using (var cmd = new NpgsqlCommand("SELECT user_login, role_id FROM users WHERE user_id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", App.CurrentUserId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                LoginLabel.Text = reader.GetString(0);
                                _roleId = reader.GetInt32(1);
                            }
                        }
                    }

                    // Получение данных из clients или workers
                    string query = _roleId == 3
                        ? "SELECT clientsurname, clientname, clientpatronymic, clientphone FROM clients WHERE user_id = @id"
                        : "SELECT worker_surname, worker_name, worker_patronymic, worker_phone FROM workers WHERE user_id = @id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", App.CurrentUserId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                SurnameEntry.Text = reader.GetString(0);
                                NameEntry.Text = reader.GetString(1);
                                PatronymicEntry.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                PhoneEntry.Text = FormatPhone(new string(reader.GetString(3).Where(char.IsDigit).ToArray()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "OK");
            }
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            string oldPass = OldPasswordEntry.Text?.Trim();
            string newPass = NewPasswordEntry.Text?.Trim();
            string confirmPass = ConfirmPasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                await DisplayAlert("Ошибка", "Все поля пароля обязательны", "OK");
                return;
            }

            if (newPass != confirmPass)
            {
                await DisplayAlert("Ошибка", "Новый пароль и подтверждение не совпадают", "OK");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    // Проверка текущего пароля
                    using (var cmd = new NpgsqlCommand("SELECT user_password FROM users WHERE user_id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", App.CurrentUserId);
                        var currentPass = (string)await cmd.ExecuteScalarAsync();

                        if (currentPass != oldPass)
                        {
                            await DisplayAlert("Ошибка", "Старый пароль неверен", "OK");
                            return;
                        }
                    }

                    // Обновление пароля
                    using (var cmd = new NpgsqlCommand("UPDATE users SET user_password = @newPass WHERE user_id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@newPass", newPass);
                        cmd.Parameters.AddWithValue("@id", App.CurrentUserId);
                        await cmd.ExecuteNonQueryAsync();
                        await DisplayAlert("Успех", "Пароль успешно обновлён", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось изменить пароль: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClientInfoClicked(object sender, EventArgs e)
        {
            string surname = SurnameEntry.Text?.Trim();
            string name = NameEntry.Text?.Trim();
            string patronymic = PatronymicEntry.Text?.Trim();
            string phoneFormatted = PhoneEntry.Text?.Trim();  // сохраняем отформатированный вид

            if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneFormatted))
            {
                await DisplayAlert("Ошибка", "Фамилия, имя и телефон обязательны", "OK");
                return;
            }


            if (!char.IsUpper(surname[0]) || !char.IsUpper(name[0]) || (!string.IsNullOrEmpty(patronymic) && !char.IsUpper(patronymic[0])))
            {
                await DisplayAlert("Ошибка", "ФИО должно начинаться с заглавной буквы", "OK");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    string query = _roleId == 3
                        ? "UPDATE clients SET clientsurname = @surname, clientname = @name, clientpatronymic = @patronymic, clientphone = @phone WHERE user_id = @id"
                        : "UPDATE workers SET worker_surname = @surname, worker_name = @name, worker_patronymic = @patronymic, worker_phone = @phone WHERE user_id = @id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@patronymic", string.IsNullOrEmpty(patronymic) ? (object)DBNull.Value : patronymic);
                        cmd.Parameters.AddWithValue("@phone", phoneFormatted);
                        cmd.Parameters.AddWithValue("@id", App.CurrentUserId);

                        await cmd.ExecuteNonQueryAsync();
                        await DisplayAlert("Успех", "Данные успешно обновлены", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось сохранить данные: {ex.Message}", "OK");
            }
        }

        private void OnPhoneTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormattingPhone) return;

            _isFormattingPhone = true;

            string digits = new string(e.NewTextValue?.Where(char.IsDigit).ToArray());

            if (digits.StartsWith("8"))
                digits = "7" + digits.Substring(1);
            else if (!digits.StartsWith("7"))
                digits = "7" + digits;

            if (digits.Length > 11)
                digits = digits.Substring(0, 11);

            PhoneEntry.Text = FormatPhone(digits);

            _isFormattingPhone = false;
        }

        private string FormatPhone(string digits)
        {
            if (digits.Length <= 1)
                return "+7";
            if (digits.Length <= 4)
                return $"+7 {digits.Substring(1)}";
            if (digits.Length <= 7)
                return $"+7 {digits.Substring(1, 3)} {digits.Substring(4)}";
            if (digits.Length <= 9)
                return $"+7 {digits.Substring(1, 3)} {digits.Substring(4, 3)}-{digits.Substring(7)}";
            return $"+7 {digits.Substring(1, 3)} {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9)}";
        }
    }
}
