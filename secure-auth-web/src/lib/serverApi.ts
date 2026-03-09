import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';

export async function fetchProtectedData() {
	const cookieStore = await cookies();
	const authToken = cookieStore.get('access_token')?.value;

	if (!authToken) {
		redirect('/login');
	}

	const res = await fetch('http://localhost/api/protected', {
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
		throw new Error('Failed to get data');
	}

	return res.json();
}
