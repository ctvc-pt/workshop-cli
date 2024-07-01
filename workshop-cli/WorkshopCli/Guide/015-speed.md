[color=white]
Para tornar a movimentação da nave mais dinâmica, vamos adicionar uma variável 
de velocidade.

1. Dentro da função "load()" adiciona as variáveis do jogador:
   [/color] [color=blue]
    velocidade = 1
   [/color] [color=white]
2. No código que escrevemos para movimentar a nave, substitui o número 1 
por "velocidade". Deverá ficar com o seguinte aspeto:
   [/color] [color=blue]
   if love.keyboard.isDown("right") then
        posicaoX = posicaoX + velocidade
    end
   [/color] [color=white]
Não te esqueças de fazer o mesmo para os lados todos e clica Alt + L para 
executar o jogo.
   [/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/

[/color] [color=white]
Deves ter visto que não alterou nada em relação ao que tínhamos.

No código dentro da função love.load(), altera o valor de velocidade para um
valor maior ou menor e vê como isso afeta a movimentação da nave. É alterando 
estes valores que vais definir se queres que a nave se mova mais rápido ou mais 
devagar.

Executa o jogo e testa como a nave se movimenta.
[/color]