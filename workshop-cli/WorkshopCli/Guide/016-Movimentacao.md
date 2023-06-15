
Já tens o jogador criado e o próximo passo é fazer a movimentação 
da nossa nave.
Vamos começar com a movimentação para a direita.
Na nossa função "update()" mete o seguinte código:

    if love.keyboard.isDown("right") then
        posicaoX = posicaoX +1
    end

Agora corre o código usando 'Alt+L' e pressiona na tecla da seta 
para a direita


     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/



Visto que já conseguimos fazer com que o jogador ande
para o lado direito, desafio-te para o fazeres andar para o restos
dos lados.

Dicas:
esquerda-> left
cima -> up
baixo -> down

Não te esqueças de verificar em que direção o jogador é movimentado,
se for horizontal usa o posicaoX, se for vertical usa o posicaoY.
E usa o código "love.graphics.getHeight()" para ter o limite
para baixo.

