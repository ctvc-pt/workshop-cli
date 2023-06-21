
Agora que já temos a nossa nave a movimentar-se para todos os lados, vamos adicionar a velocidade.

1. Dentro da função "load()" adiciona:
   velocidade = 1;

2. No codigo que fizemos à pouco, altera o número 1 por velocidade

   if love.keyboard.isDown("right") then
     posicaoX = posicaoX + velocidade
   end

Não te esqueças de fazer para os lados todos e depois clica alt + L para correr o jogo.


     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/


Deves ter visto que não alterou nada em relação ao que tinhamos.
Altera o valor da velocidade para o valores que desejas e vê que acontece.