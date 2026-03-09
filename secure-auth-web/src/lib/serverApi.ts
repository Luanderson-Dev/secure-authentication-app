import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export async function fetchProtectedData() {
	const cookieStore = await cookies();
	const authToken = cookieStore.get('access_token')?.value;

	if (!authToken) {
		redirect('/login');
	}

	const baseUrl =
		process.env.BACKEND_INTERNAL_URL || 'http://localhost:8080/api';

	const res = await fetch(`${baseUrl}/protected`, {
		method: 'GET',
		headers: {
			Cookie: `acces_token=${authToken}`,
		},
		cache: 'no-store',
	});

	if (!res.ok) {
		if (res.status === 401) {
			redirect('/login');
		}
		throw new Error('Failed to fetch secure data');
	}

	return res.json();
}
