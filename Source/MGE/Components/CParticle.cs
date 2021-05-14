using System;
using System.Collections.Generic;
using MGE.ECS;
using MGE.Graphics;

namespace MGE.Components
{
	public class CParticle : Component
	{
		public class Particle
		{
			public readonly ushort id;

			public bool ALIVE { get; internal set; }

			public Vector2 position;
			public float rotation;
			public Vector2 size;

			public Vector2 velocity;

			public Color color;
			public byte frame;
			public Vector2 drawOffset;

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
				this.drawOffset = Vector2.zero;

				this.timeAlive = 0;

				this.emitter = emitter;
			}

			public void Draw()
			{
				GFX.Draw(emitter.texture, new RectInt(frame * emitter.frameSize.x, 0, emitter.frameSize), new Rect(position + drawOffset, size), color, rotation, new Vector2(0.5f * GFX.pixelsPerUnit) * size);
			}

			public void Kill()
			{
				ALIVE = false;
				emitter.deadParticles.Enqueue(id);
			}
		}

		public ushort particlesCount { get => (ushort)particles.Length; }
		public readonly Texture texture;
		public readonly Vector2Int frameSize;

		public Action<Particle> updateParticle = (particle) => { };

		Particle[] particles;
		Queue<ushort> deadParticles;

		public CParticle(ushort particlesCount, Texture texture, Action<Particle> updateParticle)
		{
			this.particles = new Particle[particlesCount];
			this.deadParticles = new Queue<ushort>();

			for (ushort i = 0; i < particlesCount; i++)
			{
				this.particles[i] = new Particle(i, this);
				this.deadParticles.Enqueue(i);
			}

			this.texture = texture;
			this.frameSize = new Vector2Int(texture.height);

			this.updateParticle = updateParticle;
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

			if (deadParticles.Count >= particlesCount)
				entity.Destroy();
		}

		public override void Draw()
		{
			base.Draw();

			foreach (var particle in particles)
			{
				if (particle.ALIVE)
					particle.Draw();
			}
		}

		public virtual void SpawnParticle(Vector2 position, float rotation, Vector2 size, Vector2 velocity, Color color, byte frame, Vector2 drawOffset)
		{
			ushort particle = 0;

			deadParticles.TryDequeue(out particle);

			particles[particle].ALIVE = true;

			particles[particle].position = position;
			particles[particle].rotation = rotation;
			particles[particle].size = size;

			particles[particle].velocity = velocity;

			particles[particle].color = color;
			particles[particle].frame = frame;
			particles[particle].drawOffset = drawOffset;

			particles[particle].timeAlive = 0;
		}
	}
}