# Roteiro de validação no Windows

Cenários preparados para execução; não são resultados de testes executados.

1. Compilar em Release. Esperado: compilação sem erros.
2. Abrir o aplicativo. Esperado: splash, progresso e menu.
3. Abrir Nova venda e finalizar sem itens/cliente. Esperado: aviso e nenhuma venda registrada.
4. Adicionar 2 violões de R$ 650,00 e 1 teclado de R$ 1.200,00. Esperado: total de R$ 2.500,00.
5. Adicionar mais 1 violão. Esperado: mesma linha com quantidade 3; total R$ 3.150,00.
6. Remover teclado. Esperado: total R$ 1.950,00.
7. Tentar acumular mais de 999 unidades do mesmo produto. Esperado: aviso, sem alteração do carrinho.
8. Informar cliente, selecionar Pix e finalizar. Esperado: confirmação, carrinho limpo e pedido no relatório.
9. Fechar e reabrir. Esperado: pedido continua disponível com todos os itens e valores.
10. Consultar período que contém o pedido e depois período que o exclui. Esperado: totais coerentes e lista vazia na segunda consulta.
11. Informar início posterior ao fim. Esperado: aviso, preservando a consulta anterior.
12. Selecionar um pedido. Esperado: os itens exibidos correspondem ao pedido.
13. Exportar CSV e abrir em editor/planilha. Esperado: acentos, duas casas decimais e uma linha por item.
14. Iniciar outra venda e voltar. Esperado: confirmação antes de descartar.
15. Em cópia de teste dos dados, retirar permissão de escrita da pasta. Esperado: erro de gravação, carrinho preservado e nenhuma venda adicionada ao relatório.
16. Em cópia de teste, usar JSON inválido. Esperado: erro de inicialização sem sobrescrever o arquivo.
