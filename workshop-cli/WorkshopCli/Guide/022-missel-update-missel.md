
Agora vamos fazer com que os mísseis funcionem como deviam

1. No fim do teu código mete esta função

function atualizarMisseis(dt)
  for i=table.getn(misseis), 1, -1 do
    missil = misseis[i]
    missil.posicaoX = missil.posicaoX + dt * missil.velocidade
  end
end

A função "atualizarMisseis(dt)" cuida dos mísseis no jogo. Ela faz com que os mísseis se movam para a direita. É como se os mísseis estivessem a voar pelo jogo, mas desaparecendo quando vão muito longe. Assim, a função garante que os mísseis se comportem corretamente no jogo.

2. Adiciona no fim da função "update(dt)":

-- 1
if love.keyboard.isDown("space") then
    if(left) then
      missilVelocidade = missilVelocidade - velocidade/2
    elseif(right) then
      missilVelocidade = missilVelocidade + velocidade/2
    end
    gerarMissil(posicaoX + 64, posicaoY + 32, missilVelocidade)
  end
-- 2
if missilTempo > 0 then
  missilTempo = missilTempo - dt
else
  podeDisparar = true
end
-- 3
atualizarMisseis(dt)

O código inserido agora faz o seguinte:
1. Se a tecla "espaço" for pressionada, um míssil é disparado com base na velocidade do jogador. A velocidade do míssil é ajustada se o jogador estiver a mover-se para a esquerda ou direita.
2. Um temporizador é utilizado para controlar o intervalo entre os disparos de mísseis. Quando o temporizador termina, é possível disparar outro míssil.
3. A função "atualizarMisseis(dt)" é chamada para atualizar o movimento dos mísseis existentes no jogo.

Agora podes iniciar o teu jogo e divertir-te um pouco.
