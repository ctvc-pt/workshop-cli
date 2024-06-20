
Agora vamos fazer com que os mísseis funcionem como deviam

Na função "load()", adiciona:
<span style="color:cyan">missilVelocidade = 250
   podeDisparar = true</span>

No fim do teu código todo, adiciona esta função

<span style="color:purple">  function atualizarMisseis(dt)
<span style="color:cyan">for i=table.getn(misseis), 1, -1 do
            missil = misseis[i]
            missil.posicaoX = missil.posicaoX + dt * missil.velocidade
        end
   end </span>

A função "atualizarMisseis(dt)" cuida dos mísseis no jogo. Ela faz com que os mísseis se movam para a direita. É como se os mísseis estivessem a voar pelo jogo, mas desaparecendo quando vão muito longe. Assim, a função garante que os mísseis se comportem corretamente no jogo.

Agora vamos alterar o código que adicionamos no último passo na função "update(dt)":

<span style="color:cyan">if love.keyboard.isDown("space") then
        if(left) then
            missilVelocidade = missilVelocidade - velocidade/2
        elseif(right) then
            missilVelocidade = missilVelocidade + velocidade/2
        end
        if podeDisparar then
            missil = {
                posicaoX = posicaoX + 64, 
                posicaoY = posicaoY + 32, 
                width = 16, height=16, 
                velocidade = missilVelocidade, 
                imagem = missilImagem}
            table.insert(misseis, missil)
        end
    end
    atualizarMisseis(dt)

O código inserido agora faz o seguinte:
Se a tecla "espaço" for pressionada, um míssil é disparado com base na velocidade do jogador. A velocidade do míssil é ajustada se o jogador estiver a mover-se para a esquerda ou direita. O missil também é definido aqui.
A função "atualizarMisseis(dt)" é chamada para atualizar o movimento dos mísseis existentes no jogo.

Agora podes iniciar o teu jogo e divertir-te um pouco.
