using System;

namespace MGE.Physics
{
	public static class Physics
	{
		/// <summary>
		/// Speed: Slow, 3 Sqrts
		/// </summary>
		public static bool LineVsPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point, double buffer = 0.1)
		{
			var distStart = Vector2.Distance(point, lineStart);
			var distEnd = Vector2.Distance(point, lineEnd);

			var length = Vector2.Distance(lineStart, lineEnd);

			return distStart + distEnd >= length - buffer && distStart + distEnd <= length + buffer;
		}

		/// <summary>
		/// Speed: Medium
		/// </summary>
		public static bool LineVsLine(Vector2 lineAStart, Vector2 lineAEnd, Vector2 lineBStart, Vector2 lineBEnd)
		{
			var uA =
				((lineBEnd.x - lineBStart.x) * (lineAStart.y - lineBStart.y) - (lineBEnd.y - lineBStart.y) * (lineAStart.x - lineBStart.x)) /
				((lineBEnd.y - lineBStart.y) * (lineAEnd.x - lineAStart.x) - (lineBEnd.x - lineBStart.x) * (lineAEnd.y - lineAStart.y));

			var uB =
				((lineAEnd.x - lineAStart.x) * (lineAStart.y - lineBStart.y) - (lineAEnd.y - lineAStart.y) * (lineAStart.x - lineBStart.x)) /
				((lineBEnd.y - lineBStart.y) * (lineAEnd.x - lineAStart.x) - (lineBEnd.x - lineBStart.x) * (lineAEnd.y - lineAStart.y));

			// if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
			// {
			// 	var intersection = lineAStart + (uA * (lineAEnd - lineAStart));

			// 	return true;
			// }

			// return false;

			return uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1;
		}

		/// <summary>
		/// Speed: Slow, 4 LineVsLine()
		/// </summary>
		public static bool LineVsRect(Vector2 lineStart, Vector2 lineEnd, Rect rect)
		{
			var left = LineVsLine(
				lineStart, lineEnd,
				rect.position,
				new Vector2(rect.position.x, rect.position.y + rect.height)
			);

			var right = LineVsLine(
				lineStart, lineEnd,
				rect.position,
				new Vector2(rect.position.x + rect.width, rect.position.y + rect.height)
			);

			var top = LineVsLine(
				lineStart, lineEnd,
				rect.position,
				new Vector2(rect.position.x + rect.width, rect.position.y)
			);

			var bottom = LineVsLine(lineStart, lineEnd,
				new Vector2(rect.position.x, rect.position.y + rect.height),
				new Vector2(rect.position.x + rect.width)
			);

			return left || right || top || bottom;
		}

		/// <summary>
		/// Speed: Slow, 2 Sqrts
		/// </summary>
		public static bool LineVsCirc(Vector2 lineStart, Vector2 lineEnd, Vector2 circPos, double circRad)
		{
			if (CircVsPoint(circPos, circRad, lineStart) || CircVsPoint(circPos, circRad, lineEnd)) return true;

			var length = Vector2.Distance(lineStart, lineEnd);

			var dot = (((circPos.x - lineStart.x) * (lineEnd.x - lineStart.x)) + ((circPos.y - lineStart.y) * (lineEnd.y - lineStart.y))) / (length * length);

			var closest = lineStart + (dot * (lineEnd - lineStart));

			if (!LineVsPoint(lineStart, lineEnd, closest)) return false;

			var distance = Vector2.Distance(closest, circPos);

			return distance < circRad;
		}

		/// <summary>
		/// Speed: Super Fast
		/// </summary>
		public static bool RectVsPoint(Rect rect, Vector2 point)
		{
			return rect.Contains(point);
		}

		/// <summary>
		/// Speed: Super Fast
		/// </summary>
		public static bool RectVsRect(Rect rectA, Rect rectB)
		{
			return rectB.Overlaps(rectA);
		}

		/// <summary>
		/// Speed: Fast
		/// </summary>
		public static bool CircVsPoint(Vector2 circPos, double circRadius, Vector2 point)
		{
			return (circPos - point).sqrMagnitude < circRadius * circRadius;
		}

		/// <summary>
		/// Speed: Fast
		/// </summary>
		public static bool CircVsCirc(Vector2 circAPos, double circARadius, Vector2 circBPos, double circBRadius)
		{
			return (circAPos - circBPos).sqrMagnitude < (circARadius * circARadius + circBRadius * circBRadius);
		}

		/// <summary>
		/// Speed: Medium, 1 Sqrt
		/// </summary>
		public static bool CircVsRect(Vector2 circPos, double circRadius, Rect rect)
		{
			var test = Vector2.zero;

			if (circPos.x < rect.x) test.x = rect.x; // Left
			else if (circPos.x > rect.x + rect.width) test.x = rect.x + rect.width; // Right
			if (circPos.y < rect.y) test.y = rect.y; // Top
			else if (circPos.y > rect.y + rect.height) test.y = rect.y + rect.height; // Bottom

			return Vector2.Distance(circPos, test) <= circRadius;
		}

		public static RaycastHit RayVsGrid(Vector2 origin, Vector2 direction, Func<int, int, bool> isSolid, int maxIterations = 64)
		{
			var mapPos = (Vector2Int)origin;
			var sideDist = Vector2.zero;
			var deltaDist = new Vector2(Math.Abs(1.0 / direction.x), Math.Abs(1.0 / direction.y));
			var step = Vector2Int.zero;

			if (isSolid.Invoke(mapPos.x, mapPos.y))
			{
				return new RaycastHit(-step)
				{
					origin = origin,

					distance = 0.0,
					direction = direction,
				};
			}

			if (direction.x < 0)
			{
				step.x = -1;
				sideDist.x = (origin.x - mapPos.x) * deltaDist.x;
			}
			else
			{
				step.x = 1;
				sideDist.x = (mapPos.x + 1.0 - origin.x) * deltaDist.x;
			}
			if (direction.y < 0)
			{
				step.y = -1;
				sideDist.y = (origin.y - mapPos.y) * deltaDist.y;
			}
			else
			{
				step.y = 1;
				sideDist.y = (mapPos.y + 1.0 - origin.y) * deltaDist.y;
			}

			var hasHit = false;
			var side = 0;
			var interations = 0;

			while (!hasHit && interations < maxIterations)
			{
				if (sideDist.x < sideDist.y)
				{
					sideDist.x += deltaDist.x;
					mapPos.x += step.x;
					side = 0;
				}
				else
				{
					sideDist.y += deltaDist.y;
					mapPos.y += step.y;
					side = 1;
				}

				hasHit = isSolid.Invoke(mapPos.x, mapPos.y);

				interations++;
			}

			var distance = 0.0;

			if (side == 0) distance = (mapPos.x - origin.x + (1 - step.x) / 2) / direction.x;
			else distance = (mapPos.y - origin.y + (1 - step.y) / 2) / direction.y;

			if (hasHit)
			{
				// TODO: Get normals working
				return new RaycastHit(Math.Abs(direction.x) > Math.Abs(direction.y) ? new Vector2(-Math.Sign(direction.x), 0.0) : new Vector2(0.0, -Math.Sign(direction.y)))
				{
					origin = origin,

					distance = distance,
					direction = direction,
				};
			}

			return null;
		}
	}
}