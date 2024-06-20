
Para criar um jogo, precisamos de recursos como imagens, sons e fontes.
Vamos começar por carregar as imagens.

1. Adicionei várias imagens dentro da pasta "mygame", com os nomes de:
   <span style="color:orange">naves;
inimigos;
mísseis;
</span>

que usarás no jogo.
Podes clicar nas imagens para as ver.


Agora vamos desenhar o jogador.
Temos de primeiro carregar a imagem, e depois desenha-la.

Para carregar a imagem, escreve dentro da função love.load():<span style="color:cyan">imagem = love.graphics.newImage("nome-imagem.png")</span>
Não te esqueças de alterar "nome-imagem" pelo nome da nave que tu escolheste anteriormente.

Depois, escreve na função "draw()":<span style="color:cyan">love.graphics.draw(imagem, 100, 100, 0, 1, 1)</span>
Quando vires a imagem na tela de jogo, continua para o próximo passo (alt+L).


