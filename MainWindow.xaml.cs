using CompiladorAntrWinui.Exemplos;
using CompiladorAntrWinui.Gramatica;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CompiladorAntrWinui
{
    public sealed partial class MainWindow : Window
    {
        public List<Exemplo> Exemplos { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            Exemplos = MainViewModelCarregarExemplos.CarregarExemplos();

            if (Exemplos == null || Exemplos.Count == 0)
            {
                Console.WriteLine("Nenhum exemplo foi carregado.");
            }
            else
            {
                Console.WriteLine($"Exemplos carregados: {Exemplos.Count}");
            }
        }

        private async void btnCompilar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string entrada = txtEntrada.Text;

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    txtArvore.Text = "Por favor, insira um texto para compilar.";
                    txtSQL.Text = "";
                    return;
                }

                string arvore, sql;
                sql = Compilar.TraduzirParaSQL(entrada, out arvore);

                txtArvore.Text = arvore;
                txtSQL.Text = sql;
            }
            catch (Exception ex)
            {
                txtArvore.Text = $"Erro: {ex.Message}";
                txtSQL.Text = "";
            }
        }


        private async void listViewExemplos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewExemplos.SelectedItem is Exemplo exemplo)
            {
                string content = await LoadFileAsync(exemplo.Caminho);
                txtEntrada.Text = content;
            }
        }

        private async Task<string> LoadFileAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return await File.ReadAllTextAsync(filePath);
                }
                else
                {
                    txtArvore.Text = "Arquivo não encontrado.";
                    return "";
                }
            }
            catch (Exception ex)
            {
                txtArvore.Text = $"Erro ao ler o arquivo: {ex.Message}";
                return "";
            }
        }
    }
}
