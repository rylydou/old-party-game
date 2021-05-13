using System;
using System.Collections.Generic;
using MGE.ECS;
using MGE.Graphics;

namespace MGE.Components
{
	public class CParticle : Component
	{
		public struct Particle
		{
			public readonly ushort id;

			public bool ALIVE { get; private set; }

			public Vector2 position;
			public float rotation;
			public Vector2 size;

			public Vector2 velocity;

			public Color color;
			public byte frame;

			public float timeAlive;

			public readonly CParticle emitter;

			public Particle(ushort id, CParticle emitter)
			{
				this.id = id;

				this.ALIVE = false;

				this.position = Vector2.zero;
				this.rotation = 0.0f;
				this.size = Vector2.zero;

				this.velocity = Vector2.zero;

				this.color = Color.white;
				this.frame = 0;

				this.timeAlive = 0;

				this.emitter = emitter;
			}

			public void Draw()
			{
				GFX.Draw(emitter.texture, new RectInt(frame * emitter.frameSize.x, 0, emitter.frameSize), position, color);
			}

			public void Kill()
			{
				ALIVE = false;
				emitter.deadParticles.Enqueue(id);
			}
		}

		public int particlesCount { get => particles.Length; }
		public Texture texture;
		public Vector2Int frameSize;

		public Action<Particle> updateParticle = (x) => { };

		Particle[] particles;
		Queue<ushort> deadParticles;

		public override void Init()
		{
			base.Init();

			particles = new Particle[particlesCount];

			for (ushort i = 0; i < particlesCount; i++)
			{
				particles[i] = new Particle(i, this);
			}
		}

		public override void Update()
		{
			base.Update();

			ushort index = 0;
			foreach (var particle in particles)
			{
				if (particle.ALIVE)
				{
					particles[index].timeAlive += Time.deltaTime;
					updateParticle.Invoke(particle);
				}

				index++;
			}
		}

		public override void Draw()
		{
			base.Draw();

			foreach (var particle in particles)
			{
				particle.Draw();
			}
		}

		public virtual void SpawnParticle(Vector2 positio, float rotation, Vector2 size, Vector2 velocity, Color color, byte frame)
		{
			ushort particle = 0;

			deadParticles.TryDequeue(out particle);
			// TODO:
		}
	}
}